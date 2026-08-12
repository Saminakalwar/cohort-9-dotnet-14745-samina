using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TaskManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;


namespace TaskManagement.Persistence;
public static class DependencyInjection
{
   public static IServiceCollection AddPersistenceServices(
    this IServiceCollection services,
    IConfiguration configuration)
{
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

    return services;
}
}