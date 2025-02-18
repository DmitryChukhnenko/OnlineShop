using Microsoft.AspNetCore.Identity;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            const string adminEmail = "admin@example.com";
            const string adminPassword = "Admin123!";
            
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser() 
                {
                    FullName = "Dmitry Chukhenko",
                    UserName = adminEmail,
                    Email = adminEmail,
                };
                
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}