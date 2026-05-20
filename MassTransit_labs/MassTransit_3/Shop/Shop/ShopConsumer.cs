using MassTransit;
using Messages_etc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Shop_consumer : IConsumer<OrderStart>, IConsumer<Confirmation>, IConsumer<NoConfirmation>, IConsumer<AnswerAviable>, IConsumer<AnswerNotAviable>
{
    public async Task Consume(ConsumeContext<OrderStart> ctx)  //shop asking if we want to confirm the order
    {
        Custom_ConsoleCol.ConsoleWrite($"[INFO] {ctx.Message.CorrelationId} order start", ConsoleColor.Magenta);
    }
    public async Task Consume(ConsumeContext<Confirmation> ctx)  //shop asking if we want to confirm the order
    {
        Custom_ConsoleCol.ConsoleWrite($"[INFO] {ctx.Message.CorrelationId} clietn confimerd", ConsoleColor.Magenta);
    }
    public async Task Consume(ConsumeContext<NoConfirmation> ctx)  //shop asking if we want to confirm the order
    {
        Custom_ConsoleCol.ConsoleWrite($"[INFO] {ctx.Message.CorrelationId} client calnlled", ConsoleColor.Magenta);
    }
    public async Task Consume(ConsumeContext<AnswerAviable> ctx)  //shop asking if we want to confirm the order
    {
        Custom_ConsoleCol.ConsoleWrite($"[INFO] {ctx.Message.CorrelationId} warehouse confirmed", ConsoleColor.Magenta);
    }
    public async Task Consume(ConsumeContext<AnswerNotAviable> ctx)  //shop asking if we want to confirm the order
    {
        Custom_ConsoleCol.ConsoleWrite($"[INFO] {ctx.Message.CorrelationId} warehouse canceled", ConsoleColor.Magenta);
    }

}

