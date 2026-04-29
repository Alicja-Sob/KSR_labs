using MassTransit;
using Shared_Lib;

namespace Odbiorca_A
{
    public class BackgorundService_A : IConsumer<Message_1>
    {
        public Task Consume(ConsumeContext<Message_1> ctx)
        {
            var hdr1 = ctx.Headers.Get<string>("Hdr1");
            var hdr2 = ctx.Headers.Get<int>("Hdr2");

            Custom_ConsoleCol.ConsoleWrite($"[REC][A] {hdr1}: {hdr2} -> {ctx.Message.Message_Text}", ConsoleColor.Green);

            return Task.CompletedTask;

        }
    }

}



