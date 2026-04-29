using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Lib
{
    public interface IMessage_2
    {
        public string Message_Text { get; }
    }

    public record Message_2 : IMessage_2
    {
        public string Message_Text { get; set; }
    }

}
