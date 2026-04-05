using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using KSR_WCF2;

namespace WCF_1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service1 : IZadanie5, IZadanie6
    {
        public string ScalNapisy(string a, string b)
        {
            return a + b;
        }
        public void Dodaj(int a, int b)
        {
            Console.WriteLine("Dodaj called");

            var callback = OperationContext.Current.GetCallbackChannel<IZadanie6Zwrotny>();

            Console.WriteLine("Sending callback...");
            callback.Wynik(a + b);
            Console.WriteLine("Callback sent");
        }
    }
}
