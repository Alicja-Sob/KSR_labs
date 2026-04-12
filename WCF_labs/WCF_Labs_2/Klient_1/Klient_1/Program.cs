using KSR_WCF2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Klient_1
{
    public class Handler : ServiceReference2.IZadanie2Callback
    {
        public void Zadanie([MessageParameter(Name = "zadanie2")] string zadanie, int pkt, bool zaliczone)
        {
            Console.WriteLine($"{zadanie} pkt: {pkt} zaliczone: {zaliczone}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // ZADANIE 1 ---------------------------------------------------------
            var client1 = new ServiceReference1_v2.Zadanie1Client();
            Console.WriteLine("Zadanie 1 test\n");
            IAsyncResult result = client1.BeginDlugieObliczenia(null, null);

            for (int x = 0; x < 21; x++)
            {
                Console.WriteLine(client1.Szybciej(x, 3 * x * x - 2 * x));
            }
            Console.WriteLine(client1.EndDlugieObliczenia(result));
            Console.WriteLine("\n\n");

            ((IDisposable)client1).Dispose();


            // ZADANIE 2 ---------------------------------------------------------
            var client2 = new ServiceReference2.Zadanie2Client(new InstanceContext(new Handler()));
            Console.WriteLine("\nZadanie 2 test\n");

            client2.PodajZadania();
            Console.ReadKey();

            ((IDisposable)client2).Dispose();


        }
    }
}
