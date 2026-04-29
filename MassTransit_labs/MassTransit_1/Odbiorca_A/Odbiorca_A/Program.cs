using Microsoft.Extensions.Logging;
using Shared_Lib;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Odbiorca_A;

Custom_ConsoleCol.ConsoleWrite("----- Odbiorca [A] -----", ConsoleColor.Green);

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.None);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BackgorundService_A>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("kolejka_A", e =>
        {
            e.ConfigureConsumer<BackgorundService_A>(ctx);
        });
    });
});

var host = builder.Build();

Custom_ConsoleCol.ConsoleWrite("[INFO] BackgroundService A uruchomiony", ConsoleColor.Green);

await host.RunAsync();
