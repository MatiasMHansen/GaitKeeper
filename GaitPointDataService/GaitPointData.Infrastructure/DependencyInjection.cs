using Amazon.S3;
using GaitPointData.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace GaitPointData.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Service registration
            services.AddScoped<IPointDataStorage, PointDataStorage>();
            services.AddHostedService<MinioBucketInitializer>();

            // Konfiguration af Minio
            services.AddOptions<MinioOptions>()
            .BindConfiguration("Minio")
            .ValidateDataAnnotations();

            services.AddSingleton<IAmazonS3>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<MinioOptions>>().Value;
                var config = new AmazonS3Config
                {
                    ServiceURL = options.Endpoint,
                    ForcePathStyle = true
                };

                return new AmazonS3Client(options.AccessKey, options.SecretKey, config);
            });

            return services;
        }
    }

    public class MinioOptions
    {
        [Required] // [Required] er en data annotation, der sikrer at værdien er sat i appsettings.json
        public string Endpoint { get; set; } = default!;
        [Required]
        public string AccessKey { get; set; } = default!;
        [Required]
        public string SecretKey { get; set; } = default!;
        [Required]
        public string Bucket { get; set; } = default!;
    }
}
