using Abonent_A;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared_Library;

Custom_ConsoleCol.ConsoleWrite("----- Abonent [A] -----", ConsoleColor.Green);

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.None);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BackgroundServiceA>();
    x.AddConsumer<ExceptionHnadler_A>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("kolejka_A", e =>
        {
            e.ConfigureConsumer<BackgroundServiceA>(ctx);
            e.ConfigureConsumer<ExceptionHnadler_A>(ctx);
        });

    });
});

var host = builder.Build();

Custom_ConsoleCol.ConsoleWrite("[INFO] BackgroundService A uruchomiony", ConsoleColor.Green);

await host.RunAsync();