using MassTransit;
using Messages_etc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
internal class BackgroundService_WH : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var line = Console.ReadLine();
                if (int.TryParse(line, out var amount))
                {
                    Consumer_WH.AddStock(amount);
                }
            }
        }, stoppingToken);
    } 
}
  