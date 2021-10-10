using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace RotaChecker
{
    public class Shift
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public TimeSpan Length { get; }
        public bool Weekend { get; }
        public bool Night { get; }
        public int WeekNumber { get; }
        private static Calendar cal = CultureInfo.InvariantCulture.Calendar;


        public Shift(DateTime start, DateTime end)
        {
            StartTime = start;
            EndTime = end;
            Length = EndTime - StartTime;

            if(start.DayOfWeek == DayOfWeek.Saturday || start.DayOfWeek == DayOfWeek.Sunday)
            {
                Weekend = true;
            }
            else
            {
                Weekend = false;
            }

            if(start.Hour < 6 || start.Hour >= 23)
            {
                Night = true;
            }
            else
            {
                Night = false;
            }

            //assign week number
            WeekNumber = cal.GetWeekOfYear(StartTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

        }

    }
}
