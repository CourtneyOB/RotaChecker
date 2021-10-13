using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace RotaChecker
{
    public class Shift : CalendarBase
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public TimeSpan Length { get; }
        public bool Weekend { get; }
        public bool Night { get; }
        public bool Long { get;  }
        public int WeekNumber { get; }

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

            if(Length.TotalHours > 10)
            {
                Long = true;
            }
            else
            {
                Long = false;
            }

            //assign week number
            WeekNumber = Calendar.GetWeekOfYear(StartTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

        }

    }
}
