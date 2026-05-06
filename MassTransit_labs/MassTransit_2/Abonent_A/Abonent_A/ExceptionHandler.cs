using MassTransit;
using Shared_Library;
namespace Abonent_A
{
    public class ExceptionHnadler_A : IConsumer<Fault<OdpA>>
    {
        public async Task Consume(ConsumeContext<Fault<OdpA>> ctx)
        {
            foreach (var ex in ctx.Message.Exceptions)
            {
                Custom_ConsoleCol.ConsoleWrite($"[EXC] Exception -> {ex.Message}", ConsoleColor.Red);
            }
        }
    }
}
