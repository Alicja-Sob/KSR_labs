using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages_etc
{
    // INTERFACES ------------------------------------------------------------------------

    public interface IAnswerAviable : CorrelatedBy<Guid> { }
    public interface IAnswerNotAviable : CorrelatedBy<Guid> { }

    // RECORDS ---------------------------------------------------------------------------

    public record AnswerAviable(Guid CorrelationId) : IAnswerAviable { }
    public record AnswerNotAviable(Guid CorrelationId) : IAnswerNotAviable { }
}
