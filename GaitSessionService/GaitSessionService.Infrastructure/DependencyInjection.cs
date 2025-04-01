using GaitSessionService.Application;
using GaitSessionService.Application.Query;
using GaitSessionService.Infrastructure.Query;
using GaitSessionService.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace GaitSessionService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register application services here
            services.AddScoped<IGaitSessionQuery, GaitSessionQuery>();
            services.AddScoped<IGaitSessionRepository, GaitSessionRepository>();

            // Database
            services.AddDbContext<GaitSessionContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("GaitkeeperDB"), // Skal matche db navnet fra Aspire
                    x => x.MigrationsAssembly("GaitSessionService.DatabaseMigration")));

            // services.AddHttpClient(); - Skal bruges til at kalde andre services, men her ville jeg nok bruge Dapr istedet.

            return services;
        }
    }
}
