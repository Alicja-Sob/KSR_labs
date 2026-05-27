using Azure.Storage.Queues;
using Microsoft.Extensions.Options;
using System.Text.Json;

public interface IQueueService
{
    Task AddContentToQueue(string blobName);
}

public class QueueService : IQueueService
{
    private readonly QueueClient _queueClient;

    public QueueService(QueueServiceClient queueServiceClient)
    {
        _queueClient = queueServiceClient.GetQueueClient("processing-1-queue");
        _queueClient.CreateIfNotExists();
    }

    public async Task AddContentToQueue(string blobName)
    {
        var message = new FileAbtMessage { BlobName = blobName };

        var jsonMessage = JsonSerializer.Serialize(message);

        await _queueClient.SendMessageAsync(jsonMessage);
    }
}