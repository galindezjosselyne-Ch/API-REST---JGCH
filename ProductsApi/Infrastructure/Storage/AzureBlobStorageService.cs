using Azure.Storage.Blobs;
using ProductsApi.Application.Interfaces;

namespace ProductsApi.Infrastructure.Storage;

public class AzureBlobStorageService : IStorageService
{
    private readonly BlobServiceClient? _blobClient;

    public AzureBlobStorageService(string? connectionString)
    {
        if (!string.IsNullOrEmpty(connectionString))
            _blobClient = new BlobServiceClient(connectionString);
    }

    public async Task<string?> UploadFileAsync(Stream fileStream, string fileName)
    {
        if (_blobClient == null)
        {
            var tmp = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_{fileName}");
            using var fs = File.Create(tmp);
            await fileStream.CopyToAsync(fs);
            return $"file://{tmp}";
        }

        var container = _blobClient.GetBlobContainerClient("product-images");
        await container.CreateIfNotExistsAsync();
        var blobName = $"{Guid.NewGuid()}_{fileName}";
        var blob = container.GetBlobClient(blobName);
        await blob.UploadAsync(fileStream, overwrite: true);
        return blob.Uri.ToString();
    }
}
