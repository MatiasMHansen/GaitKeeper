using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using GaitPointData.Application;
using GaitPointData.Domain.Aggregate;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace GaitPointData.Infrastructure
{
    public class PointDataStorage : IPointDataStorage
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public PointDataStorage(IAmazonS3 s3Client, IOptions<MinioOptions> options)
        {
            _s3Client = s3Client;
            _bucketName = options.Value.Bucket;
        }

        public async Task SaveAsync(PointData pointData)
        {
            // 1. Lav filnavn (eks: "0c6e72d0.json")
            var key = $"{pointData.Id}.json";

            // 2. Serialiser PointData til JSON
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
        }

        public Task<PointData> LoadAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
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
