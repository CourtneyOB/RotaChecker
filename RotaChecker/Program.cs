using System;
using System.Collections.Generic;

namespace RotaChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            Shift shift1 = new Shift(new DateTime(2021, 10, 10, 23, 30, 0), new DateTime(2021, 10, 11, 9, 0, 0));
            Shift shift2 = new Shift(new DateTime(2021, 10, 13, 9, 30, 0), new DateTime(2021, 10, 13, 17, 0, 0));
            Shift shift3 = new Shift(new DateTime(2021, 10, 15, 10, 0, 0), new DateTime(2021, 10, 15, 19, 15, 0));
            Shift shift4 = new Shift(new DateTime(2021, 10, 16, 23, 30, 0), new DateTime(2021, 10, 17, 3, 0, 0));
            Shift shift5 = new Shift(new DateTime(2021, 10, 19, 4, 0, 0), new DateTime(2021, 10, 19, 9, 0, 0));

            Rota rota = new Rota(shift1, shift2, shift3, shift4, shift5);

            //List<string> text = rota.Describe();
            //foreach(string s in text)
            //{
            //    Console.WriteLine(s);
            //}

            Console.WriteLine($"The rota starts at {rota.RotaStartTime} and ends at {rota.RotaEndTime}");

            Console.WriteLine(rota.Max48PerWeek());



        }
    }
}
