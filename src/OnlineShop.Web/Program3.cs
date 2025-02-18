// using System.Text;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.IdentityModel.Tokens;
// using MudBlazor.Services;
// using OnlineShop.Domain.Entities;
// using OnlineShop.Infrastructure.Data;
// using OnlineShop.Web.Components;
// using Serilog;

// namespace OnlineShop.Web.Main;
// public class Program {
//     public static void Main(string[] args) {
//         var builder = WebApplication.CreateBuilder(args);

//         // Add services to the container.
//         builder.Services.AddRazorComponents().AddInteractiveServerComponents();
//         builder.Services.AddMudServices();

//         builder.Host.UseSerilog((ctx, lc) => lc
//             .ReadFrom.Configuration(ctx.Configuration)
//             .Enrich.WithProperty("Application", "OnlineShop")
//             .Enrich.FromLogContext()
//             .WriteTo.Console()
//             .WriteTo.Seq("http://seq"));

//         // Добавляем в сервисы
//         builder.Services.AddDbContext<ApplicationDbContext>(options =>
//             options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//         // Добавляем Identity
//         builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
//             .AddEntityFrameworkStores<ApplicationDbContext>()
//             .AddDefaultUI()
//             .AddDefaultTokenProviders();
            
//         builder.Services.AddAuthentication(options => {
//                 options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//             })
//             .AddCookie()
//             .AddJwtBearer(options =>
//             {
//                 options.TokenValidationParameters = new TokenValidationParameters
//                 {
//                     ValidateIssuer = true,
//                     ValidateAudience = true,
//                     ValidateLifetime = true,
//                     ValidateIssuerSigningKey = true,
//                     ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                     ValidAudience = builder.Configuration["Jwt:Audience"],
//                     IssuerSigningKey = new SymmetricSecurityKey(
//                         Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
//                 };
//             });

//         builder.Services.AddAuthorization(options =>
//         {
//             options.AddPolicy("AdminOnly", policy => 
//                 policy.RequireRole("Admin").RequireAuthenticatedUser());
//         });

//         // Настраиваем MediatR
//         builder.Services.AddMediatR(cfg => 
//             cfg.RegisterServicesFromAssembly(typeof(ApplicationLayer).Assembly));

//         // Добавляем FluentValidation
//         builder.Services.AddValidatorsFromAssemblyContaining<ApplicationLayer>();

//         // Настраиваем AutoMapper
//         builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

//         // Добавляем Health Checks
//         builder.Services.AddHealthChecks()
//             .AddNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
//             .AddRedis(builder.Configuration.GetConnectionString("Redis"));
        
//         var app = builder.Build();        

//         // Применяем миграции
//         using (var scope = app.Services.CreateScope())
//         {
//             var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//             db.Database.Migrate();
//         }

//         // Configure the HTTP request pipeline.
//         if (!app.Environment.IsDevelopment())
//         {
//             app.UseExceptionHandler("/Error");
//             app.UseHsts();
//         }

//         app.MapHealthChecks("/health");

//         app.UseHttpsRedirection();
//         app.UseStaticFiles();
//         app.UseRouting();
//         app.UseAuthentication();
//         app.UseAuthorization();
//         app.UseAntiforgery();

//         app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

//         app.Run();        
//     }
// }
