using MassTransit;
using Shared_Library;
namespace Abonent_A
{
    public class BackgroundServiceA : IConsumer<Publ>
    {
        public async Task Consume(ConsumeContext<Publ> ctx)
        {
            Custom_ConsoleCol.ConsoleWrite($"[REC][A] Wiadomosc -> {ctx.Message.number}", ConsoleColor.Green);

            if (ctx.Message.number % 2 == 0)
            {
                Custom_ConsoleCol.ConsoleWrite($"[SENT][A] Wiadomosc -> {ctx.Message.number}", ConsoleColor.Green);
                await ctx.Publish(new OdpA(ctx.Message.number));
            }

        }
    }
}
