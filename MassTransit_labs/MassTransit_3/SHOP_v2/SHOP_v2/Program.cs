using MassTransit;
using Messages_etc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

Custom_ConsoleCol.ConsoleWrite("-------- SHOP --------", ConsoleColor.Magenta);

var builder = Host.CreateApplicationBuilder(args);

//disable microsoft logging in console
builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.None);
builder.Logging.AddFilter("MassTransit", LogLevel.None);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

builder.Services.AddHostedService<BackgroundService_Shop>();

var host = builder.Build();
Custom_ConsoleCol.ConsoleWrite("[INFO] BackgroundService Running", ConsoleColor.Magenta);

await host.RunAsync();