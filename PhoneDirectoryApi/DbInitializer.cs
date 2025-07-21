using Microsoft.AspNetCore.Identity;
using PhoneDirectoryApi.Data;
using PhoneDirectoryApi.Models.Domain;

namespace PhoneDirectoryApi
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            PhoneDirectoryContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Apply any pending migrations
            await context.Database.EnsureCreatedAsync();

            var roles = new[] { "Admin", "Client" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@phonedirectory.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
