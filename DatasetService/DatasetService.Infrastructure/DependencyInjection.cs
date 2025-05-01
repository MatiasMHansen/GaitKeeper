
using DatasetService.Application;
using DatasetService.Application.Query;
using DatasetService.Infrastructure.Query;
using DatasetService.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DatasetService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register application services here
            services.AddScoped<IDatasetQuery, DatasetQuery>();
            services.AddScoped<IDatasetRepository, DatasetRepository>();

            return services;
        }
    }
}
