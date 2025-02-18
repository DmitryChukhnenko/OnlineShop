using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace OnlineShop.Infrastructure.Services;

public class MinioFileStorage : IFileStorageService
{
    private readonly IMinioClient _minio;
    private readonly string _bucketName = "products";

    public MinioFileStorage(IConfiguration config)
    {
        _minio = new MinioClient()
            .WithEndpoint(config["Minio:Endpoint"])
            .WithCredentials(config["Minio:AccessKey"], config["Minio:SecretKey"])
            .Build();        
    }

    private async Task EnsureBucketExists()
    {
        var exists = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
        if (!exists) await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
    }

    public async Task<string> UploadProductImage(Stream fileStream, string fileName)
    {
        await EnsureBucketExists();
        var objectName = $"{Guid.NewGuid()}-{fileName}";
        await _minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType("image/jpeg"));
        
        return $"/images/{objectName}"; // URL через Nginx
    }
}

public interface IFileStorageService {
    public Task<string> UploadProductImage(Stream fileStream, string fileName);
}