using MassTransit;
using Microsoft.Extensions.Hosting;
using Shared_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wydawca_W;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

public class ProducerService : BackgroundService
{
    private readonly IPublishEndpoint _publishEndpoint;
    private int num = 1;
    public static bool W_dziala = false;


    public ProducerService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (W_dziala)
            {
                await _publishEndpoint.Publish(new Publ(num), stoppingToken);
                Custom_ConsoleCol.ConsoleWrite($"[SENT] Wiadomosc {num}", ConsoleColor.Magenta);
                num++;
            }

            // printing out the stats
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.S)
            {
                Custom_ConsoleCol.ConsoleWrite("[INFO] Statystyki wiadomosci wg. typu:", ConsoleColor.DarkYellow);

                foreach (var type in Gathering_Stats.tries_perType.Keys.Union(Gathering_Stats.success_perType.Keys).Union(Gathering_Stats.published_perType.Keys))
                {
                    Gathering_Stats.tries_perType.TryGetValue(type, out var tries);
                    Gathering_Stats.published_perType.TryGetValue(type, out var published);
                    Gathering_Stats.success_perType.TryGetValue(type, out var success);

                    Custom_ConsoleCol.ConsoleWrite(
                        $"{type}" +
                        $"\n\tProby obslugi: {tries}, " +
                        $"\n\tPomyslna obsluga: {success}, " +
                        $"\n\tOpublikowane wiadomosci: {published}", ConsoleColor.DarkYellow
                    );

                }
            }

            await Task.Delay(1000, stoppingToken);
        }

    }


}
