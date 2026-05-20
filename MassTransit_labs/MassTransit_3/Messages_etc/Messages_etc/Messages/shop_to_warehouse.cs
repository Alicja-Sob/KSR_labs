using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages_etc
{
    // INTERFACES ------------------------------------------------------------------------

    public interface IAskAviability : CorrelatedBy<Guid> { public int amount { get; } }


    // RECORDS ---------------------------------------------------------------------------

    public record AskAviablility(Guid CorrelationId, int amount) : IAskAviability { }
}
