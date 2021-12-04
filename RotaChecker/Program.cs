using RotaChecker.Classes;
using System;
using System.Collections.Generic;

namespace RotaChecker
{
    class Program
    {
        static void Main(string[] args)
        {

            Rota rota = RotaBuilder.CreateRota();
            Compliance compliance = new Compliance(rota);

/*            List<string> text = rota.Describe();
            foreach(string s in text)
            {
               Console.WriteLine(s);
            }*/

            compliance.CheckAll();


        }

    }
}
