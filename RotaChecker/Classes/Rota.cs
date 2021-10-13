﻿using System;
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

        //Tests

        public bool Max48PerWeek()
        {
            Console.WriteLine("Checking max average 48 hours per week...");

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

        public bool Max72Per168()
        {
            Console.WriteLine("Checking max 72 hours per 168 hour period...");

            for (int i = 0; i < Length.Days; i++)
            {
                //Get midnight on the first day to be checked
                DateTime setMidnight = new DateTime(RotaStartTime.Year, RotaStartTime.Month, RotaStartTime.Day);

                //Select the next date to be cycled through and add 7 days
                DateTime startDateTime = setMidnight.AddDays(i);
                DateTime endDateTime = startDateTime.AddDays(7);

                //If there are less than 7 days remaining, then there is no need to check further
                if(DateTime.Compare(RotaEndTime, endDateTime) < 0)
                {
                    break;
                }

                double thisWeeklyHours = 0;
                Console.WriteLine($"Checking {startDateTime} to {endDateTime}");

                //Selects all the shifts with start or end time within window
                var thisWeekShifts = Shifts.Where(y =>(DateTime.Compare(startDateTime, y.StartTime) <= 0 && DateTime.Compare(endDateTime, y.StartTime) > 0) || (DateTime.Compare(startDateTime, y.EndTime) < 0 && DateTime.Compare(endDateTime, y.EndTime) >= 0));
                             
                Console.WriteLine($"{thisWeekShifts.Count()} shifts found");

                foreach(Shift s in thisWeekShifts)
                {
                    //check whether it is fully in the time period
                    if(DateTime.Compare(startDateTime, s.StartTime) <= 0 && DateTime.Compare(endDateTime, s.EndTime) >= 0)
                    {
                        thisWeeklyHours += s.Length.TotalHours;
                    }
                    else if (DateTime.Compare(startDateTime, s.StartTime) > 0 && DateTime.Compare(endDateTime, s.EndTime) >= 0)
                    {
                        
                        //starts before start but finishes after
                        TimeSpan partialShift = s.EndTime - startDateTime;
                        thisWeeklyHours += partialShift.TotalHours;
                    }
                    else if(DateTime.Compare(startDateTime, s.StartTime) <= 0 && DateTime.Compare(endDateTime, s.EndTime) < 0)
                    {
                        //starts before end but finishes after
                        TimeSpan partialShift = endDateTime - s.StartTime;
                        thisWeeklyHours += partialShift.TotalHours;
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong");
                    }
                }

                Console.WriteLine($"The 7 day period has {thisWeeklyHours} hours");

                if(thisWeeklyHours > 72)
                {
                    return false;
                }

            }
            
            return true;
        }

        public bool Max13HourShift()
        {
            Console.WriteLine("Checking max 13 hours per shift...");

            foreach (Shift s in Shifts)
            {
                if(s.Length.TotalHours > 13)
                {
                    return false;
                }
            }
            
            return true;
        }

        public bool Max4LongShifts()
        {
            
            return true;
        }

    }
}