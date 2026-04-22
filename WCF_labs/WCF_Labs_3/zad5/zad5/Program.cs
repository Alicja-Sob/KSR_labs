using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace zad5
{
    [ServiceContract]
    public interface IZadanie3
    {
        [OperationContract, WebGet(UriTemplate = "index.xhtml"), XmlSerializerFormat]
        XmlDocument Serwuj();

        [OperationContract, WebInvoke(UriTemplate = "Dodaj/{a}/{b}")]
        int Dodaj(string a, string b);

        [OperationContract, WebGet(UriTemplate = "scripts.js")]
        Stream GetStream();
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var fabryka = new ChannelFactory<IZadanie3>(
                new WebHttpBinding(),
                new EndpointAddress("http://localhost:30703/Service1.svc"));
            fabryka.Endpoint.Behaviors.Add(new WebHttpBehavior());
            var klient = fabryka.CreateChannel();

            Console.WriteLine(klient.Dodaj("1", "8"));
            ((IDisposable)klient).Dispose();
            fabryka.Close();
            Console.ReadKey();
        }
    }
}
