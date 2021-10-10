using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

namespace RotaChecker
{
    class Rota
    {
        public List<Shift> Shifts { get; set; }
        public DateTime RotaStartTime { get; set; }
        public DateTime RotaEndTime { get; set; }
        public TimeSpan Length { get;  }
        public int WeekNumberStart { get; }
        public int WeekNumberEnd { get; }
        private static Calendar cal = CultureInfo.InvariantCulture.Calendar;

        public Rota(Shift a, Shift b, Shift c, Shift d, Shift e)
        {
            Shifts = new List<Shift> { a, b, c, d, e };
            RotaStartTime = Shifts.Select(s => s.StartTime).Min();
            RotaEndTime = Shifts.Select(s => s.EndTime).Max();

            WeekNumberStart = cal.GetWeekOfYear(RotaStartTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            WeekNumberEnd = cal.GetWeekOfYear(RotaEndTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday); 

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

        public bool Max48PerWeek()
        {
            List<double> AllWeeklyHours = new List<double>();

            for(int i = WeekNumberStart; i <= WeekNumberEnd; i++)
            {
                var thisWeekShifts = Shifts.Where(s => s.WeekNumber == i);
                double thisWeeklyHours = 0;
                foreach(Shift s in thisWeekShifts)
                {
                    thisWeeklyHours += s.Length.TotalHours;
                }

                Console.WriteLine($"Week {i} contains {thisWeeklyHours} hours");

                AllWeeklyHours.Add(thisWeeklyHours);
            }

            Console.WriteLine($"The Average is {AllWeeklyHours.Average()} hours");

            if(AllWeeklyHours.Average() <= 48.0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

    }
}
