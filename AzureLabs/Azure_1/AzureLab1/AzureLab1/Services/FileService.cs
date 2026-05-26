using Azure.Storage.Blobs;
using System.Text;

public interface IFileService
{
    Task SaveFileAsync(string fileName, string content);
    Task<string?> ReadFileAsync(string fileName);
}
public class FileService : IFileService
{
    private readonly BlobContainerClient _containerClient;

    public FileService(BlobServiceClient blobServiceClient)
    {
        _containerClient = blobServiceClient.GetBlobContainerClient("files");

        _containerClient.CreateIfNotExists();
    }

    public async Task SaveFileAsync(string fileName, string content)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        await blobClient.UploadAsync(stream, overwrite: true);
    }
    public async Task<string?> ReadFileAsync(string fileName)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);

            if (!await blobClient.ExistsAsync())
                return null;

            var response = await blobClient.DownloadContentAsync();

            return response.Value.Content.ToString();
        }
        catch
        {
            return null;
        }
    }
}