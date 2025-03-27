using GaitPointData.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GaitPointData.Infrastructure
{
    public class MinioBucketInitializer : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MinioBucketInitializer> _logger;

        public MinioBucketInitializer(IServiceScopeFactory scopeFactory, ILogger<MinioBucketInitializer> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking if MinIO bucket exists...");

            using var scope = _scopeFactory.CreateScope();
            var storage = scope.ServiceProvider.GetRequiredService<IPointDataStorage>();

            await storage.EnsureBucketExistsAsync();

            _logger.LogInformation("MinIO bucket checked/created.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        // 'StopAsync' bliver ikke brugt i dette tilfælde, men er krævet af 'IHostedService' interfacet
    }
}
