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
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register application services here
            services.AddScoped<IGaitSessionQuery, GaitSessionQuery>();
            services.AddScoped<IGaitSessionRepository, GaitSessionRepository>();

            return services;
        }
    }
}
