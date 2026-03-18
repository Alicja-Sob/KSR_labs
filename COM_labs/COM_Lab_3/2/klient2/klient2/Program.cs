using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace klient2
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Type type = Type.GetTypeFromProgID("KSR20.COM3Klasa.1");    // typ COM po ProgIF

                if (type == null)
                {
                    Console.WriteLine("Something went wrong (no type)");
                    return;
                }

                object actInstance = Activator.CreateInstance(type);    // Utworzenie instancji

                // IDispatch (late binding)
                object wynik = type.InvokeMember( "Test", BindingFlags.InvokeMethod, null, actInstance, 
                    new object[] { "zad2 dziala\n" } );

                Console.WriteLine("Dlugosc przekazanego tekstu: " + wynik);
            }
            catch (Exception excpt)
            {
                Console.WriteLine("Error: " + excpt.Message);
            }
        }
    }
}
