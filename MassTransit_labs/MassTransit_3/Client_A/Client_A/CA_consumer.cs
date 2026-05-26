using MassTransit;
using Messages_etc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_A
{
    internal class CA_consumer : IConsumer<AskConfirmation>, IConsumer<AcceptOrder>, IConsumer<RejectOrder>
    {
        public Task Consume(ConsumeContext<AskConfirmation> ctx)  //shop asking if we want to confirm the order
        {
            Custom_ConsoleCol.ConsoleWrite($"[INFO] Confirm order {ctx.Message.CorrelationId}  for {ctx.Message.amount} stuff? y/n", ConsoleColor.Green);
            return Task.CompletedTask;
        }
        public Task Consume(ConsumeContext<AcceptOrder> ctx)  //the order was accepted and realized
        {
            Custom_ConsoleCol.ConsoleWrite($"[ORDER] Order {ctx.Message.CorrelationId} accepted and realized :D", ConsoleColor.Green);
            return Task.CompletedTask;
        }
        public Task Consume(ConsumeContext<RejectOrder> ctx)  //the order was no fulfilled for one reason or another
        {
            if (!Console.KeyAvailable)
                Thread.Sleep(1000);
            Custom_ConsoleCol.ConsoleWrite($"[ORDER] Order {ctx.Message.CorrelationId}  failed :[", ConsoleColor.Green);
            return Task.CompletedTask;
        }
    }
}
