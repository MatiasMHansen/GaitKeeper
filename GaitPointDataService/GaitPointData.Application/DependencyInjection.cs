using GaitPointData.Application.Command;
using GaitPointData.Application.Query;
using Microsoft.Extensions.DependencyInjection;

namespace GaitPointData.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services here
            services.AddScoped<IPointDataCommand, PointDataCommand>();
            services.AddScoped<IPartialPointDataQuery, PartialPointDataQuery>();

            return services;
        }
    }
}
