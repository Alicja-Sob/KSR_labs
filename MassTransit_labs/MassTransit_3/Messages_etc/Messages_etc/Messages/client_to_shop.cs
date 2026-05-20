using MassTransit;

namespace Messages_etc
{
    // INTERFACES ------------------------------------------------------------------------

    public interface IOrderStart : CorrelatedBy<Guid> { 
        public string client_letter { get; } 
        public int amount { get; } 
    }

    public interface IConfirmation : CorrelatedBy<Guid> { }

    public interface INoConfirmation : CorrelatedBy<Guid> { }

    // RECORDS ---------------------------------------------------------------------------

    public record OrderStart(Guid CorrelationId, string client_letter, int amount) : IOrderStart { }
    
    public record Confirmation(Guid CorrelationId) : IConfirmation { }
    
    public record NoConfirmation(Guid CorrelationId) : INoConfirmation { }
}
