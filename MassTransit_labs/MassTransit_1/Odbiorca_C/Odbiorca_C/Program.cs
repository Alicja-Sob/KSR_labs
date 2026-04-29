using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Odbiorca_C;
using Shared_Lib;

Custom_ConsoleCol.ConsoleWrite("----- Odbiorca [C] -----", ConsoleColor.Cyan);

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.None);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BackgroundService_C>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("kolejka_C", e =>
        {
            e.ConfigureConsumer<BackgroundService_C>(ctx);
        });
    });
});

var host = builder.Build();

Custom_ConsoleCol.ConsoleWrite("[INFO] BackgroundService C uruchomiony", ConsoleColor.Cyan);

await host.RunAsync();
