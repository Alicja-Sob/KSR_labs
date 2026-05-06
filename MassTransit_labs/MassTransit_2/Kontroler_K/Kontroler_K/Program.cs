using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared_Library;

Custom_ConsoleCol.ConsoleWrite("----- Kontroler [K] -----", ConsoleColor.DarkRed);

var builder = Host.CreateApplicationBuilder(args);

//disable Microsoft logging in console
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

builder.Services.AddHostedService<ControllerService>();

var host = builder.Build();
Custom_ConsoleCol.ConsoleWrite("[INFO] BackgroundService_K Running", ConsoleColor.DarkRed);

await host.RunAsync();