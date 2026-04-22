using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Routing;
using System.Text;
using System.Threading.Tasks;

namespace Router_6
{
    class Program
    {
        static void Main(string[] args)
        {
            var routePath1 = "net.pipe://localhost/service6_1";
            var routePath2 = "net.pipe://localhost/service6_2";
            var routeAdress = "net.pipe://localhost/router";

            var host = new ServiceHost(typeof(RoutingService));
            host.AddServiceEndpoint(
                typeof(IRequestReplyRouter),
                new NetNamedPipeBinding(),
                routeAdress);

            var routeConfig = new RoutingConfiguration();
            var contract = ContractDescription.GetContract(typeof(IRequestReplyRouter));
            var service61 = new ServiceEndpoint(
                contract,
                new NetNamedPipeBinding(),
                new EndpointAddress(routePath1));
            var service62 = new ServiceEndpoint(
                contract,
                new NetNamedPipeBinding(),
                new EndpointAddress(routePath2));

            var list = new List<ServiceEndpoint>();
            list.Add(service61);
            list.Add(service62);

            routeConfig.FilterTable.Add(new MatchAllMessageFilter(), list);
            host.Description.Behaviors.Add(new RoutingBehavior(routeConfig));

            host.Open();
            Console.WriteLine("Router");
            Console.ReadKey();
            host.Close();
        }
    }
}
