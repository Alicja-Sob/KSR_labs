using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wydawca_W
{
    internal class Observer_Publish : IPublishObserver
    {
        public Task PostPublish<T>(PublishContext<T> context) where T : class
        {
            var type = typeof(T).Name;
            Gathering_Stats.CountMessage(Gathering_Stats.published_perType, type);
            return Task.CompletedTask;
        }

        Task IPublishObserver.PrePublish<T>(PublishContext<T> context)
        {
            return Task.CompletedTask;
        }

        Task IPublishObserver.PublishFault<T>(PublishContext<T> context, Exception exception)
        {
            return Task.CompletedTask;
        }
    }
}
