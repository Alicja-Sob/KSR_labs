using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Server7
{
    // ZADANIE 7 (OWN EXCEPTION)
    
    [ServiceContract]
    public interface IZadanie7
    {
        [OperationContract]
        [FaultContract(typeof(Wyjatek7))]
        void RzucWyjatek7(string a, int b);
    }

    [DataContract]
    public class Wyjatek7
    {
        [DataMember] public string opis { get; set; }
        [DataMember] public string a { get; set; }
        [DataMember] public int b { get; set; }
    }

    public class Zadanie7 : IZadanie7
    {
        public void RzucWyjatek7(string a, int b)
        {
         /*   var exception = new FaultException<Wyjatek7>(
                new Wyjatek7(),
                new FaultReason("zad7 exc: " + a + b));
            throw exception;*/
         var exc = new FaultException<Wyjatek7>(
                new Wyjatek7
                {
                    opis = "blad zad7 ",
                    a = a,
                    b = b
                },
                new FaultReason(a + b)
             );
            throw exc;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            // ZADANIE 7 - OWN EXCEPTION -----------------
            var host7 = new ServiceHost(typeof(Zadanie7));

            host7.AddServiceEndpoint(
                typeof(IZadanie7),
                new NetNamedPipeBinding(),
                "net.pipe://localhost/ksr-wcf1-zad7-188861"
                );

            var b7 = host7.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (b7 == null) b7 = new ServiceMetadataBehavior();
            host7.Description.Behaviors.Add(b7);

            host7.AddServiceEndpoint(
                ServiceMetadataBehavior.MexContractName,
                MetadataExchangeBindings.CreateMexNamedPipeBinding(),
                "net.pipe://localhost/metadane7");

            host7.Open();
            Console.WriteLine("Zad 7 host is running");
            Console.ReadKey();
            host7.Close();
            // -------------------------------------------
        }
    }
}
