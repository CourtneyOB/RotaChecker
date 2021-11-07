using System;
using System.Collections.Generic;
using System.Text;
using RotaChecker.Classes;

namespace RotaChecker
{
    public class Shift : WorkDuty
    {

        public bool Night { get; }
        public bool Long { get;  }
        public bool EveningFinish { get; }


        public Shift(DateTime start, DateTime end) : base(start, end)
        {
           //this definition of nights is wrong and needs changing
            if(start.Hour < 6 || start.Hour >= 23)
            {
                Night = true;
            }
            else
            {
                Night = false;
            }

            if(Length.TotalHours > 10)
            {
                Long = true;
            }
            else
            {
                Long = false;
            }

            if(EndTime.Hour >= 23 || EndTime.Hour < 2)
            {
                EveningFinish = true;
            }
            else
            {
                EveningFinish = false;
            }


        }

    }
}
