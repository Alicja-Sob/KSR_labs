using Library.WebApi.Configuration;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// 1. Rejestracja kontrolerów (zastępuje stare AddMvc)
builder.Services.AddControllers();

// 2. Pobranie konfiguracji RabbitMQ z appsettings.json
// Zakładam, że klasa RabbitMqConfiguration przyjdzie w drugiej paczce
var rabbitConfiguration = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>();

// 3. Konfiguracja MassTransit (Wersja 8+)
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        if (rabbitConfiguration != null)
        {
            cfg.Host(new Uri(rabbitConfiguration.ServerAddress), hostConfigurator =>
            {
                hostConfigurator.Username(rabbitConfiguration.Username);
                hostConfigurator.Password(rabbitConfiguration.Password);
            });
        }

        // Zostawiamy definicję endpointu (choć WebApi zazwyczaj tylko wysyła wiadomości, 
        // zachowuję to, aby było w 100% zgodne z Twoim pierwotnym kodem)
        cfg.ReceiveEndpoint("library-webapi", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
        });
    });
});

var app = builder.Build();

// 4. Konfiguracja potoku HTTP (Request Pipeline)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Zazwyczaj w aplikacjach kontenerowych działających za proxy wyłącza się wymuszanie HTTPS,
// ale zostawiam to dla zgodności z oryginałem.
app.UseHttpsRedirection();

// Mapowanie kontrolerów (zastępuje stare app.UseMvc())
app.MapControllers();

app.Run();