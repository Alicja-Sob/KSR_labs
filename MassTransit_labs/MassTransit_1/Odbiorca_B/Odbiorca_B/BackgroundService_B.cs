using MassTransit;
using Microsoft.VisualBasic;
using Shared_Lib;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

namespace Odbiorca_B
{
    internal class BackgroundService_B : /*IConsumer<Message_1>, IConsumer<Message_2>,*/ IConsumer<Message_3>
    {
        private int _counter = 0;
   /*     public Task Consume(ConsumeContext<Message_1> ctx)
        {
            _counter++;
            Custom_ConsoleCol.ConsoleWrite($"[REC][B1] {ctx.Message.Message_Text} -> Recieved Messages: {_counter}", ConsoleColor.Yellow);

            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<Message_2> ctx)
        {
            _counter++;
            Custom_ConsoleCol.ConsoleWrite($"[REC][B2] {ctx.Message.Message_Text} -> Recieved Messages: {_counter}", ConsoleColor.Yellow);

            return Task.CompletedTask;
        }*/

        public Task Consume(ConsumeContext<Message_3> ctx)
        {
            _counter++;
            Custom_ConsoleCol.ConsoleWrite($"[REC][B3] {ctx.Message.Message_Text} -> Recieved Messages: {_counter}", ConsoleColor.Yellow);

            return Task.CompletedTask;
        }
    }

}
