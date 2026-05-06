using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Library
{
    public interface IOdpowiedz
    {
        public string kto { get; }
        public int tresc { get; }
    }

    public record OdpA(int tresc, string kto = "abonent A") : IOdpowiedz { }
    public record OdpB(int tresc, string kto = "abonent B") : IOdpowiedz { }
}
