using Client_A;
using MassTransit;
using Messages_etc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Custom_ConsoleCol.ConsoleWrite("-------- CLIENT A --------", ConsoleColor.Green);

var builder = Host.CreateApplicationBuilder(args);

//disable microsoft logging in console
builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.None);
builder.Logging.AddFilter("MassTransit", LogLevel.None);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CA_consumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("ca_queue", e =>
        {
            e.ConfigureConsumer<CA_consumer>(ctx);
        });
    });
});

builder.Services.AddHostedService<BackgroundService_CA>();

var host = builder.Build();

Custom_ConsoleCol.ConsoleWrite("[INFO] BackgroundService_CA Running", ConsoleColor.Green);

Custom_ConsoleCol.ConsoleWrite("[INFO] WHENEVER YOU WISH TO PLACE ANOTHER ORDER SIMPLY TYPE A NUMBER", ConsoleColor.Green);
Custom_ConsoleCol.ConsoleWrite("       REMEBER TO -ALWAYS- CONFIRM / DENY THE PREVIOUS ORDER BEFORE PLACING ANOTHER!", ConsoleColor.Green);

await host.RunAsync();