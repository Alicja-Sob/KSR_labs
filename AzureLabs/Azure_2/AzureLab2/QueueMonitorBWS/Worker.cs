using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

public class QueueMonitorWorker : BackgroundService
{
    private readonly ILogger<QueueMonitorWorker> _logger;
    private readonly QueueClient _queueClient;
    private readonly BlobServiceClient _blobServiceClient;

    public QueueMonitorWorker(ILogger<QueueMonitorWorker> logger, QueueServiceClient queueServiceClient, BlobServiceClient blobServiceClient)
    {
        _logger = logger;

        _queueClient = queueServiceClient.GetQueueClient("processing-1-queue");

        _blobServiceClient = blobServiceClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("\n[CUSTOM LOG] Started background worker for monitoring the queue\n");

        var inputContainer = _blobServiceClient.GetBlobContainerClient("original-files");

        var outputContainer = _blobServiceClient.GetBlobContainerClient("encoded-files");

        await outputContainer.CreateIfNotExistsAsync(cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                Response<QueueMessage> response =
                    await _queueClient.ReceiveMessageAsync(
                        visibilityTimeout: TimeSpan.FromSeconds(3),
                        cancellationToken: stoppingToken);

                QueueMessage msg = response.Value;
                if (msg != null)
                {
                    _logger.LogInformation($"\n[CUSTOM LOG] Got message with id:{ msg.MessageId}\n");
                    var payload = JsonSerializer.Deserialize<fileAbtMessage>(msg.MessageText);

                    if (payload is null)
                        continue;

                    var blobClient = inputContainer.GetBlobClient(payload.BlobName);

                    await using var stream = await blobClient.OpenReadAsync(cancellationToken: stoppingToken);

                    using var reader = new StreamReader(stream);

                    var originalText = await reader.ReadToEndAsync();

                    if (Random.Shared.Next(3) == 0)
                    {
                        throw new Exception($"\n[CUSTOM LOG] Random worker failure for {payload.BlobName}\n");
                    }

                    var encoded_msg = cipher_rot13.Encode_Rot13(originalText);
                    
                    var outputBlob = outputContainer.GetBlobClient(payload.BlobName);

                    await using var outStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(encoded_msg));

                    await outputBlob.UploadAsync(outStream, overwrite: true, cancellationToken: stoppingToken);


                    await _queueClient.DeleteMessageAsync(msg.MessageId,msg.PopReceipt, stoppingToken);
                    _logger.LogInformation("\n[CUSTOM LOG] Message encoded and deleted\n");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"\n[CUSTOM LOG] Processing error:{ex.Message}\n");
            }
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
        _logger.LogInformation("\n[CUSTOM LOG] (Graceful Shutdown)\n");
    }
}