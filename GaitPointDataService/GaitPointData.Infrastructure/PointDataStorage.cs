using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using GaitPointData.Application;
using GaitPointData.Domain.Aggregate;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace GaitPointData.Infrastructure
{
    public class PointDataStorage : IPointDataStorage
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly ILogger<PointDataStorage> _log;

        public PointDataStorage(IAmazonS3 s3Client, IOptions<MinioOptions> options, ILogger<PointDataStorage> logger)
        {
            _s3Client = s3Client;
            _bucketName = options.Value.Bucket;
            _log = logger;
        }

        public async Task SaveAsync(PointData pointData)
        {
            // 1. Lav filnavn (eks: "0c6e72d0.json")
            var key = $"{pointData.Id}.json";

            // 2. Serialiser PointData til JSON
            try
            {
                var json = JsonSerializer.Serialize(pointData, new JsonSerializerOptions
                {
                    WriteIndented = false,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                // 3. Konvertér til stream
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                // 4. Byg request
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key,
                    InputStream = stream
                };

                // 5. Upload til MinIO
                await _s3Client.PutObjectAsync(putRequest);
                _log.LogInformation($"Success! - PointData: '{key}' saved to bucket '{_bucketName}'");

            }
            catch (AmazonS3Exception ex)
            {
                _log.LogError($"EXCEPTION - PointDataStorage: AmazonS3Exception while saving '{key}': {ex.Message}");
            }
            catch (Exception ex)
            {
                _log.LogError($"EXCEPTION - PointDataStorage: General exception while saving '{key}': {ex.Message}");
            }
        }

        public Task<PointData> LoadAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            // 1. Generér filnavnet baseret på ID
            var key = $"{id}.json";

            try
            {
                // 2. Byg og udfør delete-request
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                await _s3Client.DeleteObjectAsync(deleteRequest);
                _log.LogInformation($"Success! - PointData: '{key}' deleted from bucket '{_bucketName}'");
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _log.LogError($"EXCEPTION - PointDataStorage tried to delete '{key}', but it was not found.");
            }
            catch (Exception ex)
            {
                _log.LogError($"EXCEPTION - PointDataStorage error deleting '{key}': {ex.Message}");
            }
        }

        public async Task EnsureBucketExistsAsync()
        {
            bool exists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName);
            if (!exists)
            {
                await _s3Client.PutBucketAsync(_bucketName);
            }
        }
    }
}
