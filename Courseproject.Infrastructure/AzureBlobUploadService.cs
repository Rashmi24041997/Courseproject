using Azure.Storage.Blobs;
using Courseproject.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Courseproject.Infrastructure;

public class AzureBlobUploadService : IUploadService
{
    private BlobContainerClient BlobContainerClient { get; }

    public AzureBlobUploadService()
    {
        var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        var containerName = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONTAINER_NAME");
        var blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
    }

    public async Task DeleteFileAsync(string filePath)
    {
        var blobClient = BlobContainerClient.GetBlobClient(filePath);
        await blobClient.DeleteAsync();
    }

    public async Task<byte[]> GetFileAsync(string filePath)
    {
        var blobClient = BlobContainerClient.GetBlobClient(filePath);
        var content = await blobClient.DownloadContentAsync();
        return content.Value.Content.ToArray();
    }

    public async Task<string> UploadFileAsync(IFormFile formFile)
    {
        var uniqueFilename = Guid.NewGuid().ToString() + "_" + formFile.FileName;
        await BlobContainerClient.UploadBlobAsync(uniqueFilename, formFile.OpenReadStream());
        return uniqueFilename;
    }
}
