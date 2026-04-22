using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nadawca1
{
    class LabConsumer : AsyncEventingBasicConsumer
    {
        public LabConsumer(IChannel channel) : base(channel) { }

        public override Task HandleBasicDeliverAsync(
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

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[REPLY] {message}", Console.ForegroundColor = ConsoleColor.Yellow);
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("---- Nadawca ----", Console.ForegroundColor = ConsoleColor.Yellow);
            Console.WriteLine("Press any key to send messages", Console.ForegroundColor = ConsoleColor.Yellow);
            Console.ReadKey();

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

            await channel.QueueDeclareAsync(
                queue: QUEUEname,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            string replyQueueName = (await channel.QueueDeclareAsync()).QueueName;

            var consumer = new LabConsumer(channel);
            await channel.BasicConsumeAsync(replyQueueName, true, consumer);


            // ------------------- EXERCISE 1 (10 dif messages) -------------------

            for (int i = 0; i < 10; i++)
            {
                string message = $"wiadomosc {i + 1}";
                var body = Encoding.UTF8.GetBytes(message);

                var props = new BasicProperties
                {
                    ReplyTo = replyQueueName,
                    CorrelationId = Guid.NewGuid().ToString(),
                    Headers = new Dictionary<string, object>()
                    {
                        { "head1", Encoding.UTF8.GetBytes("Exc.3") },
                        { "head2", 188861}
                    }
                };

                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: QUEUEname,
                    mandatory: false,
                    basicProperties: props,
                    body: body
                );

                Console.WriteLine($"[SENT] Sent message: {message}", Console.ForegroundColor = ConsoleColor.Yellow);
            }

            Console.WriteLine("All messages for exercise 1 sent", Console.ForegroundColor = ConsoleColor.Yellow);
       //     Console.WriteLine("\nPress any key to send PUB/SUB messages", Console.ForegroundColor = ConsoleColor.Yellow);
            Console.ReadKey();


            await channel.ExchangeDeclareAsync("abc", ExchangeType.Topic);

            for (int i = 0; i < 10; i++)
            {
                var body = Encoding.UTF8.GetBytes($"message {i + 1}");

                string routingKey = (i % 2 == 0)
                    ? "abc.def"
                    : "abc.xyz";

                await channel.BasicPublishAsync(
                    exchange: "abc",
                    routingKey: routingKey,
                    body: body
                );
            }

            Console.WriteLine("[SENT] PUB/SUB messages sent", Console.ForegroundColor = ConsoleColor.Yellow);
            Console.ReadKey();

        }
    }
}