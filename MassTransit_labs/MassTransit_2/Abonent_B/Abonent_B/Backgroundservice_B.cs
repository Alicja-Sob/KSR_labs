using MassTransit;
using Shared_Library;

namespace Abonent_B
{
    public class BackgroundServiceB : IConsumer<Publ>
    {
        public async Task Consume(ConsumeContext<Publ> ctx)
        {
            Custom_ConsoleCol.ConsoleWrite($"[REC][B] Wiadomosc -> {ctx.Message.number}", ConsoleColor.Cyan);

            if (ctx.Message.number % 3 == 0)
            {
                Custom_ConsoleCol.ConsoleWrite($"[SENT][B] Wiadomosc -> {ctx.Message.number}", ConsoleColor.Cyan);
                await ctx.Publish(new OdpB(ctx.Message.number));
            }

        }
    }
}
