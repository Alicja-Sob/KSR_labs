using Microsoft.Extensions.Azure;

var builder = Host.CreateApplicationBuilder(args);


builder.Services.AddAzureClients(clientBuilder =>
{
    var connString =
        builder.Configuration.GetConnectionString("AzureStorage");

    clientBuilder.AddBlobServiceClient(connString);
    clientBuilder.AddQueueServiceClient(connString);
});

builder.Services.AddHostedService<QueueMonitorWorker>();

var host = builder.Build();
host.Run();