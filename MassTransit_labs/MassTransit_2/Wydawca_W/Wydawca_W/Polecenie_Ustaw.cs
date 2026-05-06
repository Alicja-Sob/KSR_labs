using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared_Library;
using Microsoft.Extensions.Hosting;


public class Polecenie_Ustaw : IConsumer<Ustaw>
{
    public Task Consume(ConsumeContext<Ustaw> ctx)
    {
        ProducerService.W_dziala = ctx.Message.dziala;
        Custom_ConsoleCol.ConsoleWrite($"[INFO] Generator running: {ctx.Message.dziala}", ConsoleColor.DarkMagenta);
        return Task.CompletedTask;
    }
}
