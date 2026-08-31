using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Domain.Common;


namespace TaskManagement.Persistence.Identity;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager =
            scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new[]
        {
            AppRoles.Admin,
            AppRoles.User
        };

        foreach (var role in roles)
        {
            var roleExists =
                await roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                await roleManager.CreateAsync(
                    new IdentityRole(role));
            }
        }
    }
}