using Microsoft.EntityFrameworkCore;

namespace JwtAuthorizeTest.Server.Data;

public static class DataConfiguration
{
    public static IServiceCollection AddDataConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(cfg =>
        {
            cfg.UseNpgsql(configuration.GetConnectionString("DbConnect"));
        });
        return services;
    }
}
