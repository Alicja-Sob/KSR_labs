using MassTransit;
using MassTransit.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared_Library;
using System.Text;
using Wydawca_W;

Custom_ConsoleCol.ConsoleWrite("----- Wydawca [W] -----", ConsoleColor.Magenta);

var builder = Host.CreateApplicationBuilder(args);

//disable Microsoft logging in console
builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.None);
builder.Logging.AddFilter("MassTransit", LogLevel.None);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<Odpowiedzi>();
    x.AddConsumer<Polecenie_Ustaw>();
    x.AddConsumeObserver<Observer_Consumer>();
    x.AddPublishObserver<Observer_Publish>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("kolejka_W", e => 
        {
            e.UseMessageRetry(r => r.Interval(3, 500));
            // 3 tries with 50% chance barely publishes errors

            e.ConfigureConsumer<Odpowiedzi>(ctx);
            e.ConfigureConsumer<Polecenie_Ustaw>(ctx);
        }
        );
    });
});

builder.Services.AddHostedService<ProducerService>();

var host = builder.Build();
Custom_ConsoleCol.ConsoleWrite("[INFO] BackgroundService_W Running", ConsoleColor.Magenta);

await host.RunAsync();