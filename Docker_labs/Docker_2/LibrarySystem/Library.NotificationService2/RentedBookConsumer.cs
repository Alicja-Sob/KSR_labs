using MassTransit;
using Library.WebApi.Models;

namespace Library.NotificationService2;

public class RentedBookConsumer : IConsumer<BookRented>
{
    public Task Consume(ConsumeContext<BookRented> context)
    {
        Console.WriteLine($"The {context.Message.Title} with id: {context.Message.Id} was rented");
        return Task.CompletedTask;
    }
}