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

            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }

        public static async Task SeedAdminUserAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();

            var adminEmail = "admin@hastane.com";
            var adminPassword = "Admin123!";

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                var newAdmin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Admin",
                    TCKN = "11111111111",
                    Cellphone = "05555555555",
                    DateOfBirth = DateTime.Today.AddYears(-30)
                };

                var result = await userManager.CreateAsync(newAdmin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
