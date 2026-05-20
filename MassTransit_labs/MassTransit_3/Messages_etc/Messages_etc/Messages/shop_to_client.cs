using MassTransit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages_etc
{
    // INTERFACES ------------------------------------------------------------------------

    public interface IAskConfirmation : CorrelatedBy<Guid> { public int amount { get; } }
    public interface IAcceptOrder : CorrelatedBy<Guid> { public int amount { get; } }
    public interface IRejectOrder : CorrelatedBy<Guid> { public int amount { get; } }

    // RECORDS ---------------------------------------------------------------------------

    public record AskConfirmation(Guid CorrelationId, int amount) : IAskConfirmation { }
    public record AcceptOrder(Guid CorrelationId, int amount) : IAcceptOrder { }
    public record RejectOrder(Guid CorrelationId, int amount) : IRejectOrder { }
}
