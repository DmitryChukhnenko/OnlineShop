using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure;
using OnlineShop.Infrastructure.Data;
using OnlineShop.Infrastructure.Services;
using Serilog;
using Minio;
using Hangfire;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Hangfire.PostgreSql;
using MudBlazor.Services;
using MudBlazor;
using OnlineShop.Application.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OnlineShop.Infrastructure.HealthChecks;
using StackExchange.Redis;
using OnlineShop.Web.Components;
using OnlineShop.Infrastructure.Common;
using FluentValidation.AspNetCore;
using OnlineShop.Infrastructure.Validators;
using Microsoft.OpenApi.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineShop.Web.Main;
public class Program {
    public static async Task Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        // 1. Логирование
        builder.Host.UseSerilog((context, config) => 
            config.ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Seq("http://seq:5341"));

        // 2. База данных
        builder.Services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            ));

        // 3. Identity + Авторизация

        builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        // builder.Services.AddAuthentication(options =>
        // {
        //     options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
        //     options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
        //     options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        // }).AddCookie(IdentityConstants.ApplicationScheme, options =>
        // {
        //     options.LoginPath = "/login"; // Страница входа
        //     options.AccessDeniedPath = "/access-denied"; // Страница ошибки доступа
        // });

        builder.Services.AddAuthorization();
        builder.Services.AddHttpContextAccessor();

        // 4. Redis
        builder.Services.AddSingleton<IConnectionMultiplexer>(_ => 
            ConnectionMultiplexer.Connect(builder.Configuration["Redis:Configuration"]!));

        builder.Services.AddStackExchangeRedisCache(options => {
            options.Configuration = builder.Configuration["Redis:Configuration"];
            options.InstanceName = "OnlineShop_";
        });
            
        // 5. Hangfire
        builder.Services.AddHangfire(config => 
            config.UsePostgreSqlStorage(options => 
                options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("HangfireConnection"))));
        builder.Services.AddHangfireServer();

        // 6. MinIO
        var minioConfig = builder.Configuration.GetSection("Minio");
        if (minioConfig["Endpoint"] != null) {
            builder.Services.AddSingleton<IMinioClient>(new MinioClient()
            .WithEndpoint(minioConfig["Endpoint"])
            .WithCredentials(minioConfig["AccessKey"], minioConfig["SecretKey"])
            .Build());  
            builder.Services.AddScoped<IFileStorageService, MinioFileStorage>();
        }   

        // 7. Application Services
        builder.Services.AddScoped<IOrderService, RedisOrderService>();
        builder.Services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped<ICachingService, CachingService>();

        // 8. CQRS
        builder.Services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(OnlineShop.Application.Products.Commands.CreateProductCommand).Assembly));
        
        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        // 9. AutoMapper
        builder.Services.AddAutoMapper(typeof(OnlineShop.Application.Mappings.ProductProfile).Assembly);
        //builder.Services.AddAutoMapper( typeof(OnlineShop.Application.Mappings.ProductProfile),
                                    //    typeof(Application.Mappings.CategoryProfile),
                                    //    typeof(Application.Mappings.OrderProfile),
                                    //    typeof(OnlineShop.Application.Mappings.OrderStatusProfile),
                                    //    typeof(OnlineShop.Application.Mappings.PaymentDetailProfile),
                                    //    typeof(OnlineShop.Application.Mappings.ProductReviewProfile),
                                    //    typeof(OnlineShop.Application.Mappings.UserProfile)
                                    //   );

        // 10 FluentValidation

        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

        // 11. Health Checks
        builder.Services.AddSingleton<MinioHealthCheck>();
        builder.Services.AddHealthChecks()
            .AddNpgSql(
                connectionString: builder.Configuration.GetConnectionString("DefaultConnection")!,
                name: "PostgreSQL",
                failureStatus: HealthStatus.Degraded
            )
            .AddRedis(
                redisConnectionString: builder.Configuration["Redis:Configuration"]!,
                name: "Redis",
                failureStatus: HealthStatus.Unhealthy
            )
            .AddCheck<MinioHealthCheck>(
                "MinIO",
                failureStatus: HealthStatus.Degraded,
                tags: new[] { "storage" }
            );

        // 11. Blazor + MudBlazor
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
        builder.Services.AddMudServices(config => {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.VisibleStateDuration = 4000;
        });        

        builder.Services.AddControllers();

        // 12. Swagger
        builder.Services.AddEndpointsApiExplorer(); // Включаем API Explorer
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "OnlineShop API",
                Version = "v1",
                Description = "API для онлайн магазина",
                Contact = new OpenApiContact
                {
                    Name = "Dmitry Chukhnenko",
                    Email = "DmitryChukhnenko@yandex.ru"
                }
            });
        });

        var app = builder.Build();

        // 13. Инициализация базы данных
        using (var scope = app.Services.CreateScope()) {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            try {
                var context = services.GetRequiredService<ApplicationDbContext>();
                // Ждем, пока база станет доступна
                var retries = 10;
                while (retries-- > 0) {
                    if (context.Database.CanConnect()) break;
                    Thread.Sleep(3000);
                }
                context.Database.Migrate(); // Применяем миграции

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                if (!context.Roles.Any()) {
                    await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
                }
                await SeedData.InitializeAsync(
                    services.GetRequiredService<UserManager<ApplicationUser>>(),
                    roleManager
                );
                
            var hangfireContext = new ApplicationDbContext(
                scope.ServiceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()
            );
            hangfireContext.Database.Migrate();
            
            } catch (Exception ex) {
                logger.LogError(ex, "Ошибка при миграции или инициализации БД");
            }
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();        

        // 14. Middleware Pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // Включаем Swagger UI
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineShop API v1");
                options.RoutePrefix = "api-docs"; // Переносим Swagger UI с корня
            });
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.MapControllers();

        app.UseSerilogRequestLogging();
        app.UseHangfireDashboard("/hangfire", new DashboardOptions {
            Authorization = new[] { new HangfireAuthorizationFilter() }
        });

        app.MapHealthChecks("/health");
        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        // 15. Запуск фоновых задач
        // RecurringJob.AddOrUpdate<BackgroundJobService>("Update status", service => service.UpdateOrderStatusAsync(new Guid(("00000000-0000-0000-0000-000000000001"))), Cron.Daily);

        app.Run();
    }
}