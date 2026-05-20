using MassTransit;
using Messages_etc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Consumer_WH : IConsumer<AskAviablility>, IConsumer<AcceptOrder>, IConsumer<RejectOrder>
{
    
    
private static readonly object LockObj = new();
    private static int _free = 0;
    private static int _reserved = 0;
    private static readonly Dictionary<Guid, int> Reservations = new();
    public Task Consume(ConsumeContext<AskAviablility> context)
    {
        lock (LockObj)
        {
            PrintState();
            if (_free >= context.Message.amount)
            {
                _free -= context.Message.amount;
                _reserved += context.Message.amount;
                Reservations[context.Message.CorrelationId] =
                context.Message.amount;
                Console.WriteLine($"[WAREHOUSE] Reserved {context.Message.amount}");
            return context.Publish(new AnswerAviable(context.Message.CorrelationId));

            }
        }
        Console.WriteLine("[WAREHOUSE] Not enough stock");
        return context.Publish(new AnswerNotAviable(context.Message.CorrelationId));
    }
    public Task Consume(ConsumeContext<AcceptOrder> context)
    {
        lock (LockObj)
        {
            if (Reservations.Remove(context.Message.CorrelationId, out var
            qty))
            {
                _reserved -= qty;
            }
            PrintState();
        }
        return Task.CompletedTask;
    }
    public Task Consume(ConsumeContext<RejectOrder> context)
    {
        lock (LockObj)
        {
            if (Reservations.Remove(context.Message.CorrelationId, out var
            qty))
            {
                _reserved -= qty;
                _free += qty;
            }
            PrintState();
        }
        return Task.CompletedTask;
    }
    public static void AddStock(int amount)
    {
        lock (LockObj)
        {
            _free += amount;
            PrintState();
        }
    }
    private static void PrintState()
    {
        Console.WriteLine($"[WAREHOUSE] Free={_free}, Reserved={_reserved}");
    }
}
        /*public async Task Consume(ConsumeContext<AskAviablility> ctx)
        {
            Custom_ConsoleCol.ConsoleWrite($"[SHOP] Request for {ctx.Message.amount} stuff for order {ctx.Message.CorrelationId}", ConsoleColor.Red);
            if (BackgroundService_WH._warehouse_state.Aviable_items >= ctx.Message.amount)
            {
                Custom_ConsoleCol.ConsoleWrite($"[REQ] Stuff for order {ctx.Message.CorrelationId} reserved", ConsoleColor.Red);
                await ctx.Publish(new AnswerAviable(ctx.Message.CorrelationId));
            }
            Custom_ConsoleCol.ConsoleWrite($"[REQ] Stuff for order {ctx.Message.CorrelationId} NOT aviable :(", ConsoleColor.Red);
            await ctx.Publish(new AnswerNotAviable(ctx.Message.CorrelationId));
        }
        public async Task Consume(ConsumeContext<AcceptOrder> ctx)
        {
            BackgroundService_WH._warehouse_state.Aviable_items -= ctx.Message.amount;
            BackgroundService_WH._warehouse_state.Reserved_items += ctx.Message.amount;
            Custom_ConsoleCol.ConsoleWrite($"[SHOP] Client confirmed the order {ctx.Message.CorrelationId} :D", ConsoleColor.Red);
            //return Task.CompletedTask;
        }
        public async Task Consume(ConsumeContext<RejectOrder> ctx)
        {
            Custom_ConsoleCol.ConsoleWrite($"[SHOP] Client canceled the order {ctx.Message.CorrelationId} :/", ConsoleColor.Red);
            //return Task.CompletedTask;
        }*/

    