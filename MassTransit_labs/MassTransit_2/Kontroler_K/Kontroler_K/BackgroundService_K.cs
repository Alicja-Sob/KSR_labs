using MassTransit;
using Microsoft.Extensions.Hosting;
using Shared_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ControllerService : BackgroundService
{
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public ControllerService(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:kolejka_W"));

        while (!stoppingToken.IsCancellationRequested)
        {
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.S)
            {
                await endpoint.Send(new Ustaw(true));
                Custom_ConsoleCol.ConsoleWrite("[INFO] Pressed S -> generator started", ConsoleColor.DarkRed);
            }

            if (key == ConsoleKey.T)
            {
                await endpoint.Send(new Ustaw(false));
                Custom_ConsoleCol.ConsoleWrite("[INFO] Pressed T -> generator stopped", ConsoleColor.DarkRed);
            }


        }
    }

}