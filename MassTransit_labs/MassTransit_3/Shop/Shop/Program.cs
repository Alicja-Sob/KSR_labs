using MassTransit;
using Messages_etc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shop;
using System.Reflection.PortableExecutable;

Custom_ConsoleCol.ConsoleWrite("-------- SHOP --------", ConsoleColor.Magenta);

var builder = Host.CreateApplicationBuilder(args);

var machine = new OrderSaga();
var repo = new InMemorySagaRepository<OrderData>();

//disable microsoft logging in console
builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.None);
builder.Logging.AddFilter("MassTransit", LogLevel.None);

builder.Services.AddMassTransit(x =>
{
    //x.AddSagaStateMachine<OrderSaga, OrderData>().InMemoryRepository();

    //x.AddConsumer<Shop_consumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("shop_queue", ep =>
        {
            ep.StateMachineSaga(machine, repo);
           // ep.ConfigureConsumer<Shop_consumer>(ctx);
        });
    });
});

var host = builder.Build();

Custom_ConsoleCol.ConsoleWrite("[INFO] Shop started", ConsoleColor.Magenta);

await host.RunAsync();