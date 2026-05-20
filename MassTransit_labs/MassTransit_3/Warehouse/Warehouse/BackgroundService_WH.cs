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
  /*  private readonly IPublishEndpoint _publishEndpoint;
    public static Warehouse_state _warehouse_state = new();

    public BackgroundService_WH(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            Custom_ConsoleCol.ConsoleWrite("[INFO] Type a number if you want to increase the amount of stuff in the warehouse: ", ConsoleColor.Red);

            var input = Console.ReadLine();

            if (int.TryParse(input, out var amount))
            {

                Custom_ConsoleCol.ConsoleWrite($"[INFO {input} amount of stuff to the warehouse", ConsoleColor.Green);
                _warehouse_state.Aviable_items += amount;
                _warehouse_state.printAmounts();

            }
        }
    }
}*/