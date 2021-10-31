using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

namespace RotaChecker
{
    public class Rota : CalendarBase
    {
        public List<Shift> Shifts { get; set; }
        public DateTime RotaStartTime { get; set; }
        public DateTime RotaEndTime { get; set; }
        public TimeSpan Length { get; private set; }
        public int WeekNumberStart { get; private set; }
        public int WeekNumberEnd { get; private set; }

        //Methods

        public Rota()
        {
            Shifts = new List<Shift>();

        }        

        public List<string> Describe()
        {
            List<string> response = new List<string>();
            foreach(Shift s in Shifts)
            {
                response.Add($"The shift starts at {s.StartTime} and ends at {s.EndTime}.\nThe length of the shift is {s.Length.TotalHours} hours.");
                response.Add($"Week number is {s.WeekNumber}");

                if (s.Weekend)
                {
                   response.Add($"It is a weekend shift");
                }
                else
                {
                    response.Add($"It is not a weekend shift");
                }

                if (s.Night)
                {
                    response.Add($"It is a night shift");
                }
                else
                {
                    response.Add($"It is not a night shift");
                }
            }
            return response;
        }

        public void AddShift(Shift s)
        {
            Shifts.Add(s);
            RotaStartTime = Shifts.Select(s => s.StartTime).Min();
            RotaEndTime = Shifts.Select(s => s.EndTime).Max();
            Length = RotaEndTime - RotaStartTime;

            WeekNumberStart = Calendar.GetWeekOfYear(RotaStartTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            WeekNumberEnd = Calendar.GetWeekOfYear(RotaEndTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        }

        

    }
}
