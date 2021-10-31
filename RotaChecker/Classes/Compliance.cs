using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RotaChecker.Classes
{
    public static class Compliance

    {
        public static List<Shift> shiftsInRota = new List<Shift>();
        
        public static void CheckAll(Rota r)
        {
            shiftsInRota = GetShifts(r.Duties);

            Console.WriteLine(Max48PerWeek(r));
            Console.WriteLine(Max72Per168(r));
            Console.WriteLine(Max13HourShift(r));
            Console.WriteLine(Max4LongShifts(r));
            Console.WriteLine(Max7ConsecutiveDays(r));
            Console.WriteLine(AtLeast11HoursRest(r));
        }

        public static List<Shift> GetShifts(List<WorkDuty> duties)
        {
            List<Shift> shiftsInRota = new List<Shift>();
            foreach (WorkDuty d in duties)
            {
                if (d.GetType() == typeof(Shift))
                {
                    shiftsInRota.Add((Shift)d);
                }
            }

            return shiftsInRota;
        }

        public static bool Max48PerWeek(Rota r)
        {
            Console.WriteLine("Checking max average 48 hours per week...");

            List<double> AllWeeklyHours = new List<double>();

            for (int i = r.WeekNumberStart; i <= r.WeekNumberEnd; i++)
            {
                var thisWeekShifts = shiftsInRota.Where(s => s.WeekNumber == i);
                double thisWeeklyHours = 0;
                foreach (Shift s in thisWeekShifts)
                {
                    thisWeeklyHours += s.Length.TotalHours;
                }

                Console.WriteLine($"Week {i} contains {thisWeeklyHours} hours");

                AllWeeklyHours.Add(thisWeeklyHours);
            }

            Console.WriteLine($"The Average is {AllWeeklyHours.Average()} hours");

            if (AllWeeklyHours.Average() <= 48.0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool Max72Per168(Rota r)
        {

            Console.WriteLine("Checking max 72 hours per 168 hour period...");

            for (int i = 0; i < r.Length.Days; i++)
            {
                //Get midnight on the first day to be checked
                DateTime setMidnight = new DateTime(r.RotaStartTime.Year, r.RotaStartTime.Month, r.RotaStartTime.Day);

                //Select the next date to be cycled through and add 7 days
                DateTime startDateTime = setMidnight.AddDays(i);
                DateTime endDateTime = startDateTime.AddDays(7);

                //If there are less than 7 days remaining, then there is no need to check further
                if (DateTime.Compare(r.RotaEndTime.AddDays(1), endDateTime) < 0)
                {
                    break;
                }

                double thisWeeklyHours = 0;
                Console.WriteLine($"Checking {startDateTime} to {endDateTime}");

                //Selects all the shifts with start or end time within window
                var thisWeekShifts = shiftsInRota.Where(y => (DateTime.Compare(startDateTime, y.StartTime) <= 0 && DateTime.Compare(endDateTime, y.StartTime) > 0) || (DateTime.Compare(startDateTime, y.EndTime) < 0 && DateTime.Compare(endDateTime, y.EndTime) >= 0));

                Console.WriteLine($"{thisWeekShifts.Count()} shifts found");

                foreach (Shift s in thisWeekShifts)
                {
                    //check whether it is fully in the time period
                    if (DateTime.Compare(startDateTime, s.StartTime) <= 0 && DateTime.Compare(endDateTime, s.EndTime) >= 0)
                    {
                        thisWeeklyHours += s.Length.TotalHours;
                    }
                    else if (DateTime.Compare(startDateTime, s.StartTime) > 0 && DateTime.Compare(endDateTime, s.EndTime) >= 0)
                    {

                        //starts before start but finishes after
                        TimeSpan partialShift = s.EndTime - startDateTime;
                        thisWeeklyHours += partialShift.TotalHours;
                    }
                    else if (DateTime.Compare(startDateTime, s.StartTime) <= 0 && DateTime.Compare(endDateTime, s.EndTime) < 0)
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

                if (thisWeeklyHours > 72)
                {
                    return false;
                }

            }

            return true;
        }

        public static bool Max13HourShift(Rota r)
        {
            Console.WriteLine("Checking max 13 hours per shift...");

            foreach (Shift s in shiftsInRota)
            {
                if (s.Length.TotalHours > 13)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Max4LongShifts(Rota r)
        {

            Console.WriteLine("Checking max 4 long shifts consecutively...");

            //Cycle through all shifts
            for (int i = 0; i < shiftsInRota.Count() - 1; i++)
            {
                //If a long shift is found
                if (shiftsInRota[i].Long)
                {
                    //If there are 4 more shifts after
                    if (shiftsInRota.Count >= i + 5)
                    {

                        List<Shift> setOfFive = shiftsInRota.GetRange(i, 5);

                        //Check if consecutive

                        DateTime day1 = new DateTime(setOfFive[0].StartTime.Year, setOfFive[0].StartTime.Month, setOfFive[0].StartTime.Day);
                        DateTime day4 = new DateTime(setOfFive[3].StartTime.Year, setOfFive[3].StartTime.Month, setOfFive[3].StartTime.Day);
                        DateTime day5 = new DateTime(setOfFive[4].StartTime.Year, setOfFive[4].StartTime.Month, setOfFive[4].StartTime.Day);

                        if (DateTime.Compare(day1.AddDays(3), day4) >= 0)
                        {
                            //5 in a row returns fail test
                            if(DateTime.Compare(day1.AddDays(4), day5) >= 0)
                            {
                                if (shiftsInRota[i].Long && shiftsInRota[i + 1].Long && shiftsInRota[i + 2].Long && shiftsInRota[i + 3].Long && shiftsInRota[i + 4].Long)
                                {
                                    return false;
                                }
                            }
                            
                            //4 in a row will check whether breaks are adhered to
                            if (shiftsInRota[i].Long && shiftsInRota[i + 1].Long && shiftsInRota[i + 2].Long && shiftsInRota[i + 3].Long)
                            {

                                //Check if break will be after 4th or 5th shift

                                bool noEvenings = (!shiftsInRota[i].EveningFinish && !shiftsInRota[i + 1].EveningFinish && !shiftsInRota[i + 2].EveningFinish && !shiftsInRota[i + 3].EveningFinish);
                                bool noNights = (!shiftsInRota[i].Night && !shiftsInRota[i + 1].Night && !shiftsInRota[i + 2].Night && !shiftsInRota[i + 3].Night);

                                if (noEvenings && noNights)
                                {
                                    if (shiftsInRota.Count >= i + 6)
                                    {
                                        //Can work another
                                        List<Shift> setOfSix = shiftsInRota.GetRange(i, 6);

                                        //Check if 5th is consecutive
                                        DateTime day1b = new DateTime(setOfSix[0].StartTime.Year, setOfSix[0].StartTime.Month, setOfSix[0].StartTime.Day);
                                        DateTime day5b = new DateTime(setOfSix[4].StartTime.Year, setOfSix[4].StartTime.Month, setOfSix[4].StartTime.Day);

                                        if (DateTime.Compare(day1b.AddDays(4), day5b) >= 0)
                                        {
                                            Console.WriteLine($"Break Required after {shiftsInRota[i + 4].EndTime} - {CheckBreakRule(setOfSix)}");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Break Required after {shiftsInRota[i + 4].EndTime}");
                                        }

                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Break Required after {shiftsInRota[i + 3].EndTime} - {CheckBreakRule(setOfFive)}");
                                }
                            }
                        }

                    }

                }
            }
            return true;
        }

        public static bool CheckBreakRule(List<Shift> shifts)
        {
            Console.WriteLine("Checking breaks...");

            int breakCode = 0;
            List<Shift> setOfFour = shifts.GetRange(0, 4);

            // 0 = 48hours after 5th shift
            // 1 = 48hours after 4th shift
            // 2 = 46hours after 4th shift

            foreach (Shift s in setOfFour)
            {
                if (s.Night)
                {
                    breakCode = 2;
                    break;
                }
                if (s.EveningFinish)
                {
                    breakCode = 1;
                    break;
                }
            }

            if (breakCode == 0)
            {
                Console.WriteLine("Daytime break required (48 hours after 5th shift)");
                TimeSpan gap = shifts[5].StartTime - shifts[4].EndTime;
                if (gap.TotalHours >= 48)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (breakCode == 1)
            {
                Console.WriteLine("Evening break required (48 hours after 4th shift)");
                TimeSpan gap = shifts[4].StartTime - shifts[3].EndTime;
                if (gap.TotalHours >= 48)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (breakCode == 2)
            {
                Console.WriteLine("Night break required (46 hours)");
                TimeSpan gap = shifts[4].StartTime - shifts[3].EndTime;
                if (gap.TotalHours >= 46)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Something went wrong");
                return false;
            }

        }

        public static bool Max7ConsecutiveDays(Rota r)
        {
            //For shift in the rota
            //Check if the next 8 days are worked

            Console.WriteLine("Checking max 7 days consecutively...");

            //Cycle through all shifts
            for (int i = 0; i < shiftsInRota.Count - 1; i++)
            {
                //If there are 7 more shifts after
                if (shiftsInRota.Count >= i + 8)
                {
                    List<Shift> setOfEight = shiftsInRota.GetRange(i, 8);

                    //Check if consecutive

                    DateTime day1 = new DateTime(setOfEight[0].StartTime.Year, setOfEight[0].StartTime.Month, setOfEight[0].StartTime.Day);
                    DateTime day7 = new DateTime(setOfEight[6].StartTime.Year, setOfEight[6].StartTime.Month, setOfEight[6].StartTime.Day);
                    DateTime day8 = new DateTime(setOfEight[7].StartTime.Year, setOfEight[7].StartTime.Month, setOfEight[7].StartTime.Day);

                    if (DateTime.Compare(day1.AddDays(7), day8) >= 0)
                    {
                        Console.WriteLine($"8 consecutive shifts after {shiftsInRota[i].StartTime}");

                        return false;
                        
                    }

                    if (DateTime.Compare(day1.AddDays(6), day7) >= 0)
                    {
                        Console.WriteLine($"48 hours break required after {shiftsInRota[i+6].EndTime}");

                        TimeSpan gap = shiftsInRota[i+7].StartTime - shiftsInRota[i + 6].StartTime;

                        if(gap.TotalHours < 48)
                        {
                            return false;
                        }

                    }



                }
            }
            return true;

            //There is an exception for low intensity on-call –
            //where an on-call duty on a Saturday and Sunday contains less than 3 hours of work and no more than 3 episodes of work per day, up to 12 consecutive shifts
            //can be worked (provided that no other rule is breached).
        }

        public static bool AtLeast11HoursRest(Rota r)
        {
            Console.WriteLine("Checking 11 hours rest between shifts...");

            //Cycle through all shifts up to the last
            for (int i = 0; i < shiftsInRota.Count - 2; i++)
            {
                TimeSpan gap = shiftsInRota[i + 1].StartTime - shiftsInRota[i].EndTime;

                if(gap.TotalHours < 11)
                {
                    Console.WriteLine($"Less than 11 hours between {shiftsInRota[i].EndTime} and {shiftsInRota[i + 1].StartTime}");
                    return false;
                }
                    
            }
            return true;
        }

        //Max 1 in 2 weekends
        
        //Max 24 hour on call period (on call = non-resident)

        //No consecutive on-call periods other than Saturday and Sunday. No more than 3 on-call periods in 7 consecutive days

        //Day after an on-call period must not have work rostered longer 10 hours
        //Where Saturday and Sunday on-calls are worked consecutively, this rule applies to the day after the Sunday on-call

        //Expected rest of 8 hours per 24 hour on-call period, 5 hours must be continuous between 22:00-07:00



    }
}
