using System;
using System.Collections.Generic;

namespace RotaChecker
{
    class Program
    {
        static void Main(string[] args)
        {

            Rota rota = RotaBuilder.CreateRota();

            //List<string> text = rota.Describe();
            //foreach(string s in text)
            //{
            //    Console.WriteLine(s);
            //}


            //Length of rota period to be checking - affects average weekly hours
            Console.WriteLine($"The rota starts at {rota.RotaStartTime} and ends at {rota.RotaEndTime}. The length is {rota.Length.TotalDays} days");
            //foreach(Shift s in rota.Shifts)
            //{
            //    Console.WriteLine(s.Length.Hours);
            //}


            Console.WriteLine(rota.Max48PerWeek());

            Console.WriteLine(rota.Max72Per168());

            Console.WriteLine(rota.Max13HourShift());

            Console.WriteLine(rota.Max4LongShifts());
            //need to check night breaks (even after single night)


        }
    }
}
