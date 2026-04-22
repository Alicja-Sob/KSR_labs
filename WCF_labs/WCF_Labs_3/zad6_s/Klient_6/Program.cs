using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Klient_6
{
    [ServiceContract]
    public interface IZadanie6
    {
        [OperationContract]
        int Dodaj(int a, int b);
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var fabryka = new ChannelFactory<IZadanie6>(
                new NetNamedPipeBinding(),
                new EndpointAddress("net.pipe://localhost/router"));
            var klient = fabryka.CreateChannel();
            while (!Console.KeyAvailable)
            {
                Console.WriteLine("Wynik (klient zad6): " + klient.Dodaj(188861, 39));
                Thread.Sleep(2000);
            }
            //    Console.WriteLine("Wynik (klient zad6): " + klient.Dodaj(188861, 39));
            Console.ReadKey();
            ((IDisposable)klient).Dispose();
            fabryka.Close();
        }
    }
}
