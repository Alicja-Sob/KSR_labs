using MassTransit;
using Microsoft.Extensions.Hosting;
using Shared_Lib;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

public class ProducerService : BackgroundService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ProducerService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //while (!stoppingToken.IsCancellationRequested)
        Custom_ConsoleCol.ConsoleWrite("--> Zadanie 1-4", ConsoleColor.Magenta);

        for (int i = 0; i < 10; i++)
        {
            await _publishEndpoint.Publish(new Message_1
            {
                Message_Text = $"Wiadomosc {i + 1}"
            },
            ctx =>
            {
                ctx.Headers.Set("Hdr1", "(hEaDeR)");
                ctx.Headers.Set("Hdr2", i + 1);
            },
            stoppingToken);

            Custom_ConsoleCol.ConsoleWrite($"[SENT] Message {i + 1}", ConsoleColor.Magenta);

            await Task.Delay(1000, stoppingToken);

        }

        Custom_ConsoleCol.ConsoleWrite("\n--> Zadanie 5", ConsoleColor.Magenta);

        for (int i = 0; i < 5; i++)
        { 
            await _publishEndpoint.Publish<IMessage_2>(new Message_2
            {
                Message_Text = $"Wiadomosc nr {i+1} - typ 2"
            }, stoppingToken);

            Custom_ConsoleCol.ConsoleWrite("[SENT] Message type 2", ConsoleColor.Magenta);

            await Task.Delay(1000, stoppingToken);
        }

        Custom_ConsoleCol.ConsoleWrite("\n--> Zadanie 6", ConsoleColor.Magenta);

        for (int i = 0; i < 5; i++)
        {
            await _publishEndpoint.Publish(new Message_3
            {
                Message_Text = $"Wiadomosc nr {i + 1} - typ 3"
            }, stoppingToken);

            Custom_ConsoleCol.ConsoleWrite("[SENT] Message type 3", ConsoleColor.Magenta);

            await Task.Delay(1000, stoppingToken);
        }

    }
}