using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Odbiorca_B;
using Shared_Lib;

Custom_ConsoleCol.ConsoleWrite("----- Odbiorca [B] -----", ConsoleColor.Yellow);

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.None);
var consumer = new BackgroundService_B();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BackgroundService_B>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("kolejka_B", e =>
        {
            e.Instance(consumer); 
        });
    });
});

var host = builder.Build();

Custom_ConsoleCol.ConsoleWrite("[INFO] BackgroundService B uruchomiony", ConsoleColor.Yellow);

await host.RunAsync();
