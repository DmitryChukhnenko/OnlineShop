using Microsoft.Extensions.Diagnostics.HealthChecks;
using Minio;
using Minio.DataModel.Args;

namespace OnlineShop.Infrastructure.HealthChecks
{
    public class MinioHealthCheck : IHealthCheck
    {
        private readonly IMinioClient _minioClient;

        public MinioHealthCheck(IMinioClient minioClient)
        {
            _minioClient = minioClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Проверяем доступность бакета
                var args = new BucketExistsArgs()
                    .WithBucket("products");
                var exists = await _minioClient.BucketExistsAsync(args);
                
                return exists 
                    ? HealthCheckResult.Healthy("MinIO connection OK") 
                    : HealthCheckResult.Degraded("Bucket not found");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("MinIO connection FAILED", ex);
            }
        }
    }
}