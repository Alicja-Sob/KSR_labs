using Abonent_B;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared_Library;

Custom_ConsoleCol.ConsoleWrite("----- Abonent [B] -----", ConsoleColor.Cyan);

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.None);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BackgroundServiceB>();
    x.AddConsumer<ExceptionHandleer_B>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("kolejka_B", e =>
        {
            e.ConfigureConsumer<BackgroundServiceB>(ctx);
            e.ConfigureConsumer<ExceptionHandleer_B>(ctx);
        });
    });
});

var host = builder.Build();

Custom_ConsoleCol.ConsoleWrite("[INFO] BackgroundService B uruchomiony", ConsoleColor.Cyan);

await host.RunAsync();