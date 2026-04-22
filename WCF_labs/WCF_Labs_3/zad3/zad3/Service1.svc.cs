using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace zad3
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceContract]
    public interface IZadanie3
    {
        [OperationContract]
        [WebGet(UriTemplate = "index.xhtml", BodyStyle = WebMessageBodyStyle.Bare)]
        Stream Serwuj();

        [OperationContract]
        [WebGet(UriTemplate = "Dodaj/{a}/{b}")]
        int DodajGet(string a, string b);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Dodaj/{a}/{b}")]
        int DodajPost(string a, string b);

        [OperationContract, WebGet(UriTemplate = "scripts.js")]
        Stream GetStream();
    }

    public class Service1 : IZadanie3
    {
        string indexFile = "C:\\Users\\sobal\\OtherCodingProjects\\KSR_lab\\WCF_labs\\WCF_Labs_3\\index.xhtml";
        string scriptFile = "C:\\Users\\sobal\\OtherCodingProjects\\KSR_lab\\WCF_labs\\WCF_Labs_3\\scripts.js";
        public Stream Serwuj()
        {
            if (!File.Exists(indexFile))
                throw new WebFaultException(System.Net.HttpStatusCode.NotFound);

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return new FileStream(indexFile, FileMode.Open, FileAccess.Read);
        }

        public int Dodaj(string a, string b)
        {
            return Int32.Parse(a) + Int32.Parse(b);
        }

        public int DodajGet(string a, string b)
        {
            return Dodaj(a, b);
        }

        public int DodajPost(string a, string b)
        {
            return Dodaj(a, b);
        }

        public Stream GetStream()
        {
            if (File.Exists(scriptFile))
            {
                return new FileStream(scriptFile, FileMode.Open);
            }
            return null;
        }
    }
}
