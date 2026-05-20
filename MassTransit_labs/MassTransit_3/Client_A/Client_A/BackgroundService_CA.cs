using MassTransit;
using Messages_etc;
using Microsoft.Extensions.Hosting;

public class BackgroundService_CA : BackgroundService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public BackgroundService_CA(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            System.Threading.Thread.Sleep(1000); //TODO FIX THIS - DOESNT WORK PROPERLY
            Custom_ConsoleCol.ConsoleWrite("[INFO] Enter order amount", ConsoleColor.Green);

            var input = Console.ReadLine();
            var id = NewId.NextGuid();

            if (int.TryParse(input, out var amount))
            {
                id = NewId.NextGuid();

                Custom_ConsoleCol.ConsoleWrite($"[SENT] Placing order {id} for {amount} stuff", ConsoleColor.Green);

                await _publishEndpoint.Publish(new OrderStart(id, "A", amount), stoppingToken);

                var inputt = Console.ReadLine();

                if (inputt == "y" || inputt == "Y")
                {
                    Custom_ConsoleCol.ConsoleWrite($"[INFO] Confirming order{id}", ConsoleColor.Green);
                    await _publishEndpoint.Publish(new Confirmation(id));
                }
                else if (inputt == "n" || inputt == "N")
                {
                    Custom_ConsoleCol.ConsoleWrite($"[INFO] Cancelling order {id}", ConsoleColor.Green);
                    await _publishEndpoint.Publish(new NoConfirmation(id));
                }
            }
        }
    }
}