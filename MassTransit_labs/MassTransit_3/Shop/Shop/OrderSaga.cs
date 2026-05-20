using MassTransit;
using Messages_etc;
using System.Linq.Expressions;

namespace Shop
{
    public class OrderData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; } 
        public string CurrentState { get; set; } = string.Empty;

        public int amount { get; set; }
        public string client { get; set; } = string.Empty;

        public bool clientConfirmed {  get; set; }
        public bool WareHouseConfirmed {  get; set; }
        public bool OrderAccepted { get; set; }
        public ConsoleColor messageColor { get; set; }

        //public Guid TimeoutTokenId { get; set; }
    }

    public class OrderSaga : MassTransitStateMachine<OrderData>
    {
        public State WaitingForResponses { get; private set; }

        public Event<OrderStart> OrderStart { get; private set; }
        public Event<Confirmation> ClientConfirmation { get; private set; }
        public Event<NoConfirmation> ClientRejection { get; private set; }

        public Event<AnswerAviable> WarehouseOk { get; private set; }
        public Event<AnswerNotAviable> WarehouseNo { get; private set; }

        public OrderSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderStart, x =>
            {
                x.SelectId(ctx => ctx.Message.CorrelationId);
                x.CorrelateById(ctx => ctx.Message.CorrelationId);
            });

            Event(() => ClientConfirmation, x =>
            {
                x.SelectId(ctx => ctx.Message.CorrelationId);
                x.CorrelateById(ctx => ctx.Message.CorrelationId);
            });

            Event(() => ClientRejection, x =>
            {
                x.SelectId(ctx => ctx.Message.CorrelationId);
                x.CorrelateById(ctx => ctx.Message.CorrelationId);
            });

            //Event(() => ClientConfirmation, x => x.CorrelateById(m => m.Message.CorrelationId));
            //Event(() => ClientRejection, x => x.CorrelateById(m => m.Message.CorrelationId));

            Event(() => WarehouseOk, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => WarehouseNo, x => x.CorrelateById(m => m.Message.CorrelationId));


            Initially(
                When(OrderStart)
                    .Then(ctx =>
                    {
                        //Custom_ConsoleCol.ConsoleWrite($"[DEBUG] Starting order {ctx.Message.CorrelationId}", ConsoleColor.Yellow);
                        ctx.Saga.amount = ctx.Message.amount;
                        ctx.Saga.client = ctx.Message.client_letter;

                        ctx.Saga.clientConfirmed = false;
                        ctx.Saga.WareHouseConfirmed = false;
                        ctx.Saga.OrderAccepted = false;

                        if (ctx.Message.client_letter == "A")
                        {
                            ctx.Saga.messageColor = ConsoleColor.Green;
                        }
                        else
                        {
                            ctx.Saga.messageColor = ConsoleColor.Cyan;
                        }

                        Custom_ConsoleCol.ConsoleWrite($"[ORDER_{ctx.Saga.client}] Started order {ctx.Saga.CorrelationId} with the amount: {ctx.Saga.amount}. Waiting for confirmation", ConsoleColor.Magenta);
                    })
                    .Send(ctx =>
                        {
                            var queue = ctx.Saga.client == "A"
                                ? "queue:ca_queue"
                                : "queue:cb_queue";

                            return new Uri(queue);
                        }, ctx =>
                        new AskConfirmation(ctx.Saga.CorrelationId, ctx.Saga.amount))
                    .Send(new Uri("queue:wh_queue"), ctx =>
                        new AskAviablility(ctx.Saga.CorrelationId, ctx.Saga.amount))
                    .TransitionTo(WaitingForResponses)
                    //.Then (ctx => Custom_ConsoleCol.ConsoleWrite($"[DEBUG] Transfering to waiting for response for order {ctx.Message.CorrelationId}", ConsoleColor.Yellow))
            );


            During(WaitingForResponses,
               
            When(ClientRejection)
                    .Then(ctx => {
                        ctx.Saga.clientConfirmed = false;
                        Custom_ConsoleCol.ConsoleWrite("[INFO] Client canceled the order", ctx.Saga.messageColor);
                    })
                    .Send(ctx =>
                    {
                        var queue = ctx.Saga.client == "A"
                            ? new Uri("queue:ca_queue")
                            : new Uri("queue:cb_queue");

                        return queue;
                    }, ctx => new RejectOrder(ctx.Saga.CorrelationId, ctx.Saga.amount))
                    .Send(new Uri("queue:wh_queue"), ctx => new RejectOrder(ctx.Saga.CorrelationId, ctx.Saga.amount))
                    .Finalize(),

                When(ClientConfirmation)
                    .Then(ctx => {
                        ctx.Saga.clientConfirmed = true;
                        Custom_ConsoleCol.ConsoleWrite("[INFO] Client confirmed the order", ctx.Saga.messageColor);
                    })
                    .If(ctx => ctx.Saga.WareHouseConfirmed,
                        x => x
                            .Send(ctx =>
                            {
                                var queue = ctx.Saga.client == "A"
                                    ? new Uri("queue:ca_queue")
                                    : new Uri("queue:cb_queue");

                                return queue;
                            },
                            ctx => new AcceptOrder(ctx.Saga.CorrelationId, ctx.Saga.amount))
                            .Send(new Uri("queue:wh_queue"),
                                ctx => new AcceptOrder(ctx.Saga.CorrelationId, ctx.Saga.amount))
                            .Finalize()
                    )
                    .Then(ctx =>
                    {
                        Custom_ConsoleCol.ConsoleWrite(
                            $"[INFO] Order {ctx.Saga.CorrelationId} accepted for realization :D",
                            ctx.Saga.messageColor);
                    }),

                When(WarehouseOk)
                    .Then(ctx => {
                        ctx.Saga.WareHouseConfirmed = true;
                        Custom_ConsoleCol.ConsoleWrite("[INFO] Warehouse confirmed aviablility", ctx.Saga.messageColor);
                    })
                    .If(ctx => ctx.Saga.clientConfirmed,
                        x => x
                            .Send(ctx =>
                            {
                                var queue = ctx.Saga.client == "A"
                                    ? new Uri("queue:ca_queue")
                                    : new Uri("queue:cb_queue");

                                return queue;
                            },
                            ctx => new AcceptOrder(ctx.Saga.CorrelationId, ctx.Saga.amount))
                            .Send(new Uri("queue:wh_queue"),
                                ctx => new AcceptOrder(ctx.Saga.CorrelationId, ctx.Saga.amount))
                            .Finalize()
                    )
                    .Then(ctx =>
                    {
                        Custom_ConsoleCol.ConsoleWrite(
                            $"[INFO] Order {ctx.Saga.CorrelationId} accepted for realization :D",
                            ctx.Saga.messageColor);
                    }),

                When(WarehouseNo)
                    .Then(ctx => {
                        ctx.Saga.WareHouseConfirmed = false;
                        Custom_ConsoleCol.ConsoleWrite("[INFO] Sufficient stock not aviable in warehouse", ctx.Saga.messageColor);
                    })

                    .Send(ctx =>
                    {
                        var queue = ctx.Saga.client == "A"
                            ? new Uri("queue:ca_queue")
                            : new Uri("queue:cb_queue");

                        return queue;
                    }, ctx => new RejectOrder(ctx.Saga.CorrelationId, ctx.Saga.amount))
                    .Send(new Uri("queue:wh_queue"), ctx => new RejectOrder(ctx.Saga.CorrelationId, ctx.Saga.amount))
                    .Finalize()
            );



            SetCompletedWhenFinalized();
        }

    }
}
