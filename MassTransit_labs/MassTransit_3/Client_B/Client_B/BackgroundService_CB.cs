using MassTransit;
using Messages_etc;
using Microsoft.Extensions.Hosting;

public class BackgroundService_CB : BackgroundService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public BackgroundService_CB(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {

            var input = Console.ReadLine();
            var id = NewId.NextGuid();

            if (int.TryParse(input, out var amount))
            {
                id = NewId.NextGuid();

                Custom_ConsoleCol.ConsoleWrite($"[SENT] Placing order {id} for {amount} stuff", ConsoleColor.Cyan);

                await _publishEndpoint.Publish(new OrderStart(id, "B", amount), stoppingToken);

                var inputt = Console.ReadLine();

                if (inputt == "y" || inputt == "Y")
                {
                    Custom_ConsoleCol.ConsoleWrite($"[INFO] Confirming order{id}", ConsoleColor.Cyan);
                    await _publishEndpoint.Publish(new Confirmation(id));
                }
                else if (inputt == "n" || inputt == "N")
                {
                    Custom_ConsoleCol.ConsoleWrite($"[INFO] Cancelling order {id}", ConsoleColor.Cyan);
                    await _publishEndpoint.Publish(new NoConfirmation(id));
                }
            }
        }
    }
}