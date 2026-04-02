using Amazon.S3;
using Amazon.S3.Model;
using BePopJwt.Business.Services.StorageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BePopJwt.API.Services.Storage
{
    public class S3AudioStorageService(IAmazonS3 s3Client, IConfiguration configuration) : IAudioStorageService
    {
        public async Task<string> UploadSongAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            var bucketName = configuration["AWS:S3:BucketName"];
            if (string.IsNullOrWhiteSpace(bucketName))
            {
                throw new InvalidOperationException("AWS:S3:BucketName ayarı bulunamadı.");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var key = $"songs/{Guid.NewGuid():N}{extension}";

            await using var stream = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType,
                AutoCloseStream = true,
            };

            await s3Client.PutObjectAsync(request, cancellationToken);

            var cdnBase = configuration["AWS:S3:CdnBaseUrl"];
            if (!string.IsNullOrWhiteSpace(cdnBase))
            {
                return $"{cdnBase.TrimEnd('/')}/{key}";
            }

            return $"https://{bucketName}.s3.amazonaws.com/{key}";
        }
    }
}

