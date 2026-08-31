using CompanyProfile.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyProfile.Infrastructure.Data.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        const string roleName = "Admin";
        const string userName = "admin";
        const string password = "Admin@123456";

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        var user = await userManager.FindByNameAsync(userName);

        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = userName,
                Email = "admin@companyprofile.local",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to create admin user: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(user, roleName))
        {
            var result = await userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to assign role '{roleName}': " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}