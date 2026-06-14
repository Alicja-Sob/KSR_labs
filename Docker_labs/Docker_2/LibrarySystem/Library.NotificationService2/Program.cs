using Library.NotificationService2;
using Library.NotificationService2.Configuration;
using Library.WebApi.Models;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// 1. Rejestracja konfiguracji z appsettings.json
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));

// 2. Konfiguracja MassTransit (Wersja 8+)
builder.Services.AddMassTransit(x =>
{
    // Rejestracja konsumera
    x.AddConsumer<RentedBookConsumer>();

    // Konfiguracja RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        // Pobranie zmapowanych ustawień ze wstrzykiwania zależności
        var rabbitConfig = context.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;

        cfg.Host(new Uri(rabbitConfig.ServerAddress), hostConfigurator =>
        {
            hostConfigurator.Username(rabbitConfig.Username);
            hostConfigurator.Password(rabbitConfig.Password);
        });

        // Konfiguracja endpointu nasłuchującego
        cfg.ReceiveEndpoint("notification-service", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<RentedBookConsumer>(context);
        });
    });
});

var app = builder.Build();

// 3. Konfiguracja potoku HTTP (Minimal API)
app.MapGet("/", () => "Notification Service started!");

app.Run();