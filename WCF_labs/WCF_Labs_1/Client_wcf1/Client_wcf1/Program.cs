using KSR_WCF1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client_wcf1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ChannelFactory<IZadanie1>(
            new NetNamedPipeBinding(),
            new EndpointAddress("net.pipe://localhost/ksr-wcf1-test")
            );

            var client = factory.CreateChannel();

            Console.WriteLine(client.Test("zad1_test"));

            try
            {
                client.RzucWyjatek(true);
            }
            catch (FaultException<Wyjatek> exc)
            {
                Console.WriteLine(client.OtoMagia(exc.Detail.magia));
            }

            ((IDisposable)client).Dispose();
            factory.Close();

            //ZAD 3 TEST
            var client2 = new ServiceReference1.Zadanie2Client();
            Console.WriteLine(client2.Test("zad3 test from client"));
            ((IDisposable)client2).Dispose();
            //   Console.ReadLine();



            // ZAD 7 TEST (CATCHING OWN EXCEPTION)
            var factory7 = new ChannelFactory<ServiceReference2.IZadanie7>(
            new NetNamedPipeBinding(),
            new EndpointAddress("net.pipe://localhost/ksr-wcf1-zad7-188861")
            );

            var client7 = factory7.CreateChannel();

            //var client7 = new ServiceReference2.Zadanie7Client();
            try
            {
                Console.WriteLine("trying to throw own exteption");
                client7.RzucWyjatek7("zad7 ", 7);
            }
            catch (FaultException<ServiceReference2.Wyjatek7> exc)
            {
                Console.WriteLine("own exception caught: " + exc.Detail.opis + " " + exc.Detail.a +  " " + exc.Detail.b);
            }

            ((IDisposable)client7).Dispose();
   
        }
    }
}
