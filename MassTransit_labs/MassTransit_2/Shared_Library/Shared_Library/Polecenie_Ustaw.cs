using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Library
{
    public interface IUstaw
    {
        public bool dziala { get; }
    }

    public record Ustaw(bool dziala) : IUstaw { }
}
