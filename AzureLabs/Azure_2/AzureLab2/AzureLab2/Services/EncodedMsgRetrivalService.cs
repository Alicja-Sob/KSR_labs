using Azure.Storage.Blobs;

public interface IEncodedMsgRetrivalService
{
    Task<string> GetEncodedMessageFromQueueAsync(string blobName);
}

public class EncodedMsgRetrivalService : IEncodedMsgRetrivalService
{
    private readonly BlobContainerClient _containerClient;

    public EncodedMsgRetrivalService(BlobServiceClient blobServiceClient)
    {
        _containerClient =
            blobServiceClient.GetBlobContainerClient("encoded-files");

        _containerClient.CreateIfNotExists();
    }

    public async Task<string> GetEncodedMessageFromQueueAsync(string blobName)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);

        var download = await blobClient.DownloadContentAsync();

        return download.Value.Content.ToString();
    }
}