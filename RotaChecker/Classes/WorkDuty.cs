using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace RotaChecker
{
    public class WorkDuty : CalendarBase
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public TimeSpan Length { get; }
        public bool Weekend { get; }
        public int WeekNumber { get; }

        public WorkDuty(DateTime start, DateTime end)
        {
            StartTime = start;
            EndTime = end;
            Length = EndTime - StartTime;

            if (start.DayOfWeek == DayOfWeek.Saturday || start.DayOfWeek == DayOfWeek.Sunday)
            {
                Weekend = true;
            }
            else
            {
                Weekend = false;
            }


            //assign week number
            WeekNumber = Calendar.GetWeekOfYear(StartTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

        }

    }
}
