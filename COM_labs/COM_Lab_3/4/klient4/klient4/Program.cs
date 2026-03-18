using KlasaLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class klient4
{
        public uint Test(string napis)
        {
            try
            {
                var obj = new Klasa();
                obj.Test(napis);
                return 0;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
                return 1;
            }
        }

        static void Main(string[] args)
        {
            klient4 klient = new klient4();
            Console.WriteLine(klient.Test("Testowanie, zadanie 4 ok!"));
        }
    }
