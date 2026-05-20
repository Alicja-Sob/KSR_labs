using MassTransit;
using Messages_etc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Custom_ConsoleCol.ConsoleWrite("-------- WAREHOUSE --------", ConsoleColor.Red);

var builder = Host.CreateApplicationBuilder(args);

//disable microsoft logging in console
builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.None);
builder.Logging.AddFilter("MassTransit", LogLevel.None);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<Consumer_WH>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("wh_queue", e =>
        {
            e.ConfigureConsumer<Consumer_WH>(ctx);
        });
    });
});
builder.Services.AddHostedService<BackgroundService_WH>();

var host = builder.Build();

Custom_ConsoleCol.ConsoleWrite("[INFO] BackgroundService Running", ConsoleColor.Red);
Custom_ConsoleCol.ConsoleWrite($"[INFO] WHENEVER YOU WISH TO INCREASE THE STOCK, SIMPLY TYPE A DESIRED NUMBER", ConsoleColor.Red);

await host.RunAsync();