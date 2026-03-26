using KSR_WCF1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Server_wcf1
{
    public class Zadanie2 : IZadanie2
    {
        public string Test(string arg)
        {
            return "Zad2_test: " + arg;
        }
    }

    class Program
    {
        static void Main()
        {
            var host = new ServiceHost(typeof(Zadanie2));

            host.AddServiceEndpoint(
                typeof(KSR_WCF1.IZadanie2),
                new NetNamedPipeBinding(),
                "net.pipe://localhost/ksr-wcf1-zad2"
            );

            // ZADANIE 3 - METADANE ----------------------

            var b = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (b == null) b = new ServiceMetadataBehavior();
            host.Description.Behaviors.Add(b);

            host.AddServiceEndpoint(
                ServiceMetadataBehavior.MexContractName,
                MetadataExchangeBindings.CreateMexNamedPipeBinding(),
                "net.pipe://localhost/metadane");

            // -------------------------------------------

            // ZADANIE 4 - DIFFERENT ADDRESS -------------
            host.AddServiceEndpoint(typeof(KSR_WCF1.IZadanie2),
                new NetTcpBinding(),
                "net.tcp://127.0.0.1:55765");

            // -------------------------------------------

            host.Open();
            Console.WriteLine("Server (zad2/4) is running");
            Console.ReadKey();
            host.Close();


        }
    }
}
