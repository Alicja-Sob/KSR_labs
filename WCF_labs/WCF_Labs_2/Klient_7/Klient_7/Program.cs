using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Klient_7
{
    public class Handler : ServiceReference1.IZadanie6Callback
    {
        public void Wynik(int wyn)
        {
            Console.WriteLine($"\nWynik zwrotny zadania 6: {wyn}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // ZADANIE 5 ---------------------------------------------------------
            var client5 = new ServiceReference1.Zadanie5Client();
            Console.WriteLine(client5.ScalNapisy("zadanie 5", " zaliczone"));
            //Console.ReadKey();

            // ZADANIE 6 ---------------------------------------------------------
            var client6 = new ServiceReference1.Zadanie6Client(new InstanceContext(new Handler()));
            client6.Dodaj(1, 1);
            Console.ReadKey();

            ((IDisposable)client5).Dispose();
            ((IDisposable)client6).Dispose();
        }
    }

}
