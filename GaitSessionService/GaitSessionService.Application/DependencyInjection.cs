using GaitSessionService.Application.Command;
using Microsoft.Extensions.DependencyInjection;

namespace GaitSessionService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services here
            services.AddScoped<IGaitSessionCommand, GaitSessionCommand>();

            return services;
        }
    }
}
