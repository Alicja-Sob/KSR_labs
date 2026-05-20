using Client_B;
using MassTransit;
using Messages_etc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Custom_ConsoleCol.ConsoleWrite("-------- CLIENT B --------", ConsoleColor.Cyan);

var builder = Host.CreateApplicationBuilder(args);

//disable microsoft logging in console
builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.None);
builder.Logging.AddFilter("MassTransit", LogLevel.None);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CB_consumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("cb_queue", e =>
        {
            e.ConfigureConsumer<CB_consumer>(ctx);
        });
    });
});
builder.Services.AddHostedService<BackgroundService_CB>();

var host = builder.Build();

Custom_ConsoleCol.ConsoleWrite("[INFO] BackgroundService Running", ConsoleColor.Cyan);

Custom_ConsoleCol.ConsoleWrite("[INFO] WHENEVER YOU WISH TO PLACE ANOTHER ORDER SIMPLY TYPE A NUMBER", ConsoleColor.Cyan);
Custom_ConsoleCol.ConsoleWrite("       REMEBER TO CONFIRM / DENY THE PREVIOUS ORDER BEFORE PLACING ANOTHER!", ConsoleColor.Cyan);

await host.RunAsync();