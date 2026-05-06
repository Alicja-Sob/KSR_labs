using MassTransit;
using Shared_Library;
namespace Abonent_B
{
    public class ExceptionHandleer_B : IConsumer<Fault<OdpB>>
    {
        public async Task Consume(ConsumeContext<Fault<OdpB>> ctx)
        {
            foreach (var ex in ctx.Message.Exceptions)
            {
                Custom_ConsoleCol.ConsoleWrite($"[EXC] Exception -> {ex.Message}", ConsoleColor.Red);
            }
        }
    }
}
