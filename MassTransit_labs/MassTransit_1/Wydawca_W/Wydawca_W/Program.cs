using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared_Lib;

Custom_ConsoleCol.ConsoleWrite("----- Wydawca [W] -----", ConsoleColor.Magenta);

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

builder.Services.AddHostedService<ProducerService>();

var host = builder.Build();
Custom_ConsoleCol.ConsoleWrite("[INFO] BackgroundService Running", ConsoleColor.Magenta);

await host.RunAsync();