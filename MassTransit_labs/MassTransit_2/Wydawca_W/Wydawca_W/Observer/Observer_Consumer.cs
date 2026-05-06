using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wydawca_W
{
    public class Observer_Consumer : IConsumeObserver
    {
        Task IConsumeObserver.ConsumeFault<T>(ConsumeContext<T> context, Exception exception)
        {
            return Task.CompletedTask;
        }

        Task IConsumeObserver.PostConsume<T>(ConsumeContext<T> context)
        {
            var type = typeof(T).Name;
            Gathering_Stats.CountMessage(Gathering_Stats.success_perType, type);
            return Task.CompletedTask;
        }

        Task IConsumeObserver.PreConsume<T>(ConsumeContext<T> context)
        {
            var type = typeof(T).Name;
            Gathering_Stats.CountMessage(Gathering_Stats.tries_perType, type);
            return Task.CompletedTask;
        }
    }
}
