using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Library
{
    public interface IMessage_Publ
    {
        public int number { get; }
    }

    public record Publ(int number) : IMessage_Publ { }
}
