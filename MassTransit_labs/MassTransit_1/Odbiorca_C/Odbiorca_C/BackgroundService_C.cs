using MassTransit;
using Shared_Lib;

namespace Odbiorca_C
{
    internal class BackgroundService_C : IConsumer<IMessage_2>
    {
        public Task Consume(ConsumeContext<IMessage_2> ctx)
        {
            Custom_ConsoleCol.ConsoleWrite($"[REC][C] {ctx.Message.Message_Text}", ConsoleColor.Cyan);

            return Task.CompletedTask;

        }
    }

}
