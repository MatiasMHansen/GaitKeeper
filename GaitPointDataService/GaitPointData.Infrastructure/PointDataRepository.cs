using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using GaitPointData.Application;
using GaitPointData.Application.Query.QueryDTOs;
using GaitPointData.Domain.Aggregate;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace GaitPointData.Infrastructure
{
    public class PointDataRepository : IPointDataRepository
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly ILogger<PointDataRepository> _log;

        public PointDataRepository(IAmazonS3 s3Client, IOptions<MinioOptions> options, ILogger<PointDataRepository> logger)
        {
            _s3Client = s3Client;
            _bucketName = options.Value.Bucket;
            _log = logger;
        }

        async Task IPointDataRepository.SaveAsync(PointData pointData)
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
                throw;
            }
            catch (Exception ex)
            {
                _log.LogError($"EXCEPTION - PointDataStorage: General exception while saving '{key}': {ex.Message}");
                throw;
            }
        }

        async Task<QueryPointDataDTO> IPointDataRepository.LoadAsync(Guid id)
        {
            // 1. Konstruér nøgle baseret på ID
            var key = $"{id}.json";

            try
            {
                // 2. Hent objektet fra S3/MinIO
                var response = await _s3Client.GetObjectAsync(_bucketName, key);

                // 3. Læs stream-indholdet som tekst (JSON)
                using var reader = new StreamReader(response.ResponseStream);
                var json = await reader.ReadToEndAsync();

                // 4. Deserialisér JSON til PointData-aggregate
                var pointData = JsonSerializer.Deserialize<QueryPointDataDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // 5. Returnér objektet eller kast fejl hvis det mislykkes
                return pointData ?? throw new Exception("Deserialization failed");
            }
            catch (AmazonS3Exception ex)
            {
                _log.LogError($"EXCEPTION - LoadAsync: AmazonS3Exception while loading '{key}': {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _log.LogError($"EXCEPTION - LoadAsync: General error while loading '{key}': {ex.Message}");
                throw;
            }
        }

        async Task IPointDataRepository.DeleteAsync(Guid id)
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
                throw;
            }
            catch (Exception ex)
            {
                _log.LogError($"EXCEPTION - PointDataStorage error deleting '{key}': {ex.Message}");
                throw;
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
