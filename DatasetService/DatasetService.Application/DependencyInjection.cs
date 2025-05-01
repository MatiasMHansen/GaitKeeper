
using DatasetService.Application.Command;
using DatasetService.Application.Utility;
using Microsoft.Extensions.DependencyInjection;

namespace DatasetService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services here
            services.AddScoped<IDatasetCommand, DatasetCommand>();
            services.AddScoped<IExportDataset, ExportDataset>();

            return services;
        }
    }
}
