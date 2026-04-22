using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Odbiorca_1
{
    class LabMessConsumer : AsyncDefaultBasicConsumer
    {
        public LabMessConsumer(IChannel channel) : base(channel) { }

        public override async Task HandleBasicDeliverAsync(
            string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string exchange,
            string routingKey,
            IReadOnlyBasicProperties properties,
            ReadOnlyMemory<byte> body,
            CancellationToken cancellationToken = default)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());

            if (properties.Headers != null)
            {
                string head1 = Encoding.UTF8.GetString((byte[])properties.Headers["head1"]);
                long head2 = Convert.ToInt64(properties.Headers["head2"]);

                Console.WriteLine($"[RECV] {head1} ({head2}) : {message}", Console.ForegroundColor = ConsoleColor.Cyan);
            }
            else
            {
                Console.WriteLine($"[RECV][noHeaders] {message}", Console.ForegroundColor = ConsoleColor.Cyan);
            }

            await Task.Delay(2000);

            // RPC RESPONSE
            if (!string.IsNullOrEmpty(properties.ReplyTo))
            {
                var responseText = $"OK: {message}";
                var responseBytes = Encoding.UTF8.GetBytes(responseText);

                var replyProps = new BasicProperties
                {
                    CorrelationId = properties.CorrelationId
                };

                await Channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: properties.ReplyTo,
                    mandatory: false,
                    basicProperties: replyProps,
                    body: responseBytes
                );
            }

            await Channel.BasicAckAsync(deliveryTag, false);
        }
    }
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("---- Odbiorca 2 ----", Console.ForegroundColor = ConsoleColor.Cyan);

            string QUEUEname = "kolejka_1";

            var factory = new ConnectionFactory()
            {
                UserName = "djvdhyvp",
                Password = "Ho1B5YRHR6RXMGD4j1deR15pVX8TxPJn",
                HostName = "goose.rmq2.cloudamqp.com",
                VirtualHost = "djvdhyvp"
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            AsyncDefaultBasicConsumer consumer = new LabMessConsumer(channel);

            await channel.QueueDeclareAsync(
                queue: QUEUEname,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
            await channel.BasicQosAsync(0, 1, false);

            await channel.BasicConsumeAsync(QUEUEname, false, consumer);

         //   Console.ReadKey();


            await channel.ExchangeDeclareAsync("abc", ExchangeType.Topic);

            var topicQueue = await channel.QueueDeclareAsync();

            await channel.QueueBindAsync(topicQueue.QueueName, "abc", "*.xyz");

            var topicConsumer = new LabMessConsumer(channel);
            await channel.BasicConsumeAsync(topicQueue.QueueName, false, topicConsumer);

       //     Console.WriteLine("PUB/SUB consumer ready", Console.ForegroundColor = ConsoleColor.Cyan);

            Console.ReadKey();
        }

    }
}
