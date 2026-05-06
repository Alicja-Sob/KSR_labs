using MassTransit;
using MassTransit.Transports;
using Shared_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wydawca_W
{
    public class Odpowiedzi: IConsumer<OdpA>, IConsumer<OdpB>
    {
        public Task Consume(ConsumeContext<OdpA> ctx)
        {
            Random random = new Random();
            if (random.Next(2) == 0)
            {
                Custom_ConsoleCol.ConsoleWrite($"[EXC] Thrown exception for message {ctx.Message.tresc}", ConsoleColor.DarkGreen);
                throw new Exception("Blad przy odbiorze odpowiedzi");
            }

            Custom_ConsoleCol.ConsoleWrite($"[REC] Response from {ctx.Message.kto} -> {ctx.Message.tresc}", ConsoleColor.Green);
            return Task.CompletedTask;

        }

        public Task Consume(ConsumeContext<OdpB> ctx)
        {
            Random random = new Random();
            if (random.Next(2) == 0)
            {
                Custom_ConsoleCol.ConsoleWrite($"[EXC] Thrown exception for message {ctx.Message.tresc}", ConsoleColor.DarkRed);
                throw new Exception("Blad przy odbiorze odpowiedzi");
            }

            Custom_ConsoleCol.ConsoleWrite($"[REC] Response from {ctx.Message.kto} -> {ctx.Message.tresc}", ConsoleColor.Cyan);
            return Task.CompletedTask;

        }

    }
}
