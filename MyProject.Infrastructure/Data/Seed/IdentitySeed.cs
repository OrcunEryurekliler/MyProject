using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyProject.Core.Entities;

namespace MyProject.Infrastructure.Data.Seed
{
    public static class IdentitySeed
    {
        private static readonly string[] Roles = new[] { "Admin", "Doctor", "Patient" };

        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

            foreach (var roleName in Roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }
        }

        public static async Task SeedAdminUserAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

            // Admin bilgileri
            var adminEmail = "admin@hastane.com";
            var adminPassword = "Admin123!";

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Sistem Yöneticisi",
                    TCKN = "11111111111",
                    Cellphone = "05555555555",
                    DateOfBirth = DateTime.Today.AddYears(-30),
                };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
                // Hata durumlarını log’layabilirsiniz
            }
        }
    }
}
