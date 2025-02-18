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

namespace OnlineShop.Web.Main;
public class Program {
    public static async Task Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        // 1. Логирование
        builder.Host.UseSerilog((context, config) => 
            config.ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Seq("http://seq"));

        // 2. База данных
        builder.Services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            ));

        // 3. Identity + JWT

        builder.Services.AddIdentityCore<ApplicationUser>(options => 
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole<Guid>>() // Явное указание типа роли
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));

        builder.Services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = key
            };
        });

        // 4. Redis
        builder.Services.AddSingleton<IConnectionMultiplexer>(_ => 
            ConnectionMultiplexer.Connect(builder.Configuration["Redis:Configuration"]!));

        builder.Services.AddStackExchangeRedisCache(options => {
            options.Configuration = builder.Configuration["Redis:Configuration"];
            options.InstanceName = "OnlineShop_";
        });

        // 5. Hangfire
        builder.Services.AddHangfire(config => 
            config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
        builder.Services.AddHangfireServer();

        // 6. MinIO
        builder.Services.AddSingleton<IMinioClient>(new MinioClient()
            .WithEndpoint(builder.Configuration["MINIO_ENDPOINT"])
            .WithCredentials(
                builder.Configuration["MINIO_ACCESS_KEY"],
                builder.Configuration["MINIO_SECRET_KEY"])
            .Build());

        // 7. Application Services
        builder.Services.AddScoped<IFileStorageService, MinioFileStorage>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IOrderService, RedisOrderService>();

        // 8. CQRS
        builder.Services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(OnlineShop.Application.DependencyInjection).Assembly));

        builder.Services.AddValidatorsFromAssembly(typeof(OnlineShop.Application.DependencyInjection).Assembly);

        // 9. AutoMapper
        builder.Services.AddAutoMapper(typeof(OnlineShop.Application.Common.MappingProfile));

        // 10. Health Checks
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


        var app = builder.Build();

        // 12. Инициализация базы данных
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

        // 13. Middleware Pipeline
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        app.UseSerilogRequestLogging();
        app.UseHangfireDashboard("/hangfire", new DashboardOptions {
            Authorization = new[] { new HangfireAuthorizationFilter() }
        });

        app.MapHealthChecks("/health");
        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        // 14. Запуск фоновых задач
        RecurringJob.AddOrUpdate<IProductService>("cleanup-products", 
            s => s.CleanupOldProductsAsync(), 
            Cron.Daily);

        app.Run();

    }
}