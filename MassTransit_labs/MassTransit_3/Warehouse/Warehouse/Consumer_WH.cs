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
    private static int _aviable = 0;
    private static int _reserved = 0;
    private static readonly Dictionary<Guid, int> Reservations = new();
 
    public Task Consume(ConsumeContext<AskAviablility> ctx)
    {
        lock (LockObj)
        {
            PrintState();
            if (_aviable >= ctx.Message.amount)
            {
                _aviable -= ctx.Message.amount;
                _reserved += ctx.Message.amount;
                Reservations[ctx.Message.CorrelationId] = ctx.Message.amount;
                Custom_ConsoleCol.ConsoleWrite($"[ORDER] Reserved {ctx.Message.amount} stuff for order {ctx.Message.amount} ", ConsoleColor.Red);
                return ctx.Publish(new AnswerAviable(ctx.Message.CorrelationId));

            }
        }
        Custom_ConsoleCol.ConsoleWrite($"[ORDER] NOT enough stuff for order {ctx.Message.amount}", ConsoleColor.Red);
        return ctx.Publish(new AnswerNotAviable(ctx.Message.CorrelationId));
    }
    public Task Consume(ConsumeContext<AcceptOrder> ctx)
    {
        lock (LockObj)
        {
            if (Reservations.Remove(ctx.Message.CorrelationId, out var qty))
            {
                _reserved -= qty;
                Custom_ConsoleCol.ConsoleWrite($"[ORDER] Order {ctx.Message.amount} accepted for fullfilment :D", ConsoleColor.Red);
            }
            PrintState();
        }
        return Task.CompletedTask;
    }
    public Task Consume(ConsumeContext<RejectOrder> ctx)
    {
        lock (LockObj)
        {
            if (Reservations.Remove(ctx.Message.CorrelationId, out var
            qty))
            {
                Custom_ConsoleCol.ConsoleWrite($"[ORDER] Order {ctx.Message.amount} failed :(", ConsoleColor.Red);
                _reserved -= qty;
                _aviable += qty;
            }
            PrintState();
        }
        return Task.CompletedTask;
    }
    public static void AddStock(int amount)
    {
        lock (LockObj)
        {
            Custom_ConsoleCol.ConsoleWrite($"[INFO] Added {amount} stuff to our stock :)", ConsoleColor.Red);
            _aviable += amount;
            PrintState();
        }
    }
    private static void PrintState()
    {
        Custom_ConsoleCol.ConsoleWrite($"[INFO] Warehouse stock\n\t Aviable stuff: {_aviable}\n\t Reserved stuff: {_reserved}", ConsoleColor.Red);
    }
}

    