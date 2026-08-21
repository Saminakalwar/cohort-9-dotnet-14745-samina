using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TaskManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Persistence;
public static class DependencyInjection
{
   public static IServiceCollection AddPersistenceServices(
    this IServiceCollection services,
    IConfiguration configuration)
{
    if (services is null)
    {
        throw new ArgumentNullException(nameof(services));
    }

    if (configuration is null)
    {
        throw new ArgumentNullException(nameof(configuration));
    }

    var connectionString =
        configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException(
            "Connection string 'DefaultConnection' is not configured.");
    }

    services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(
            connectionString,
            sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure();
            });
    });

    services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

    return services;
}
}