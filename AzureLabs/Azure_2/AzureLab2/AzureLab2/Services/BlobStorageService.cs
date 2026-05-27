using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using System.Text;

public interface IBlobStorageService
{
    Task SaveContentToBlobAsync(string blobName, string content);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    
    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _containerClient = blobServiceClient.GetBlobContainerClient("original-files");

        _containerClient.CreateIfNotExists();
    }

    public async Task SaveContentToBlobAsync(string blobName, string content)
    {
        var blobbClient = _containerClient.GetBlobClient(blobName);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        await blobbClient.UploadAsync(stream, overwrite: true);
    }
}