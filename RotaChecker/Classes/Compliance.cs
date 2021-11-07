using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RotaChecker.Classes
{
    public static class Compliance

    {
        private static List<Shift> ShiftsInRota = new List<Shift>();
        private static List<OnCallPeriod> OnCallInRota = new List<OnCallPeriod>();
        private static Rota Rota;
        
        public static void CheckAll(Rota r)
        {
            Rota = r;
            ShiftsInRota = GetShifts(Rota.Duties);
            OnCallInRota = GetOnCalls(Rota.Duties);

            Console.WriteLine(Max48PerWeek());
            Console.WriteLine(Max72Per168());
            Console.WriteLine(Max13HourShift());
            Console.WriteLine(Max4LongShifts());
            Console.WriteLine(Max7ConsecutiveDays());
            Console.WriteLine(AtLeast11HoursRest());
            Console.WriteLine(NightRestBreaks());
            Console.WriteLine(WeekendFrequency());
            Console.WriteLine(Max24HourOnCall());
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

        public static List<OnCallPeriod> GetOnCalls(List<WorkDuty> duties)
        {
            List<OnCallPeriod> onCallInRota = new List<OnCallPeriod>();
            foreach (WorkDuty d in duties)
            {
                if (d.GetType() == typeof(OnCallPeriod))
                {
                    onCallInRota.Add((OnCallPeriod)d);
                }
            }

            return onCallInRota;
        }

        //These only work through CheckAll function

        private static bool Max48PerWeek()
        {
            Console.WriteLine("Checking max average 48 hours per week...");

            List<double> AllWeeklyHours = new List<double>();

            for (int i = Rota.WeekNumberStart; i <= Rota.WeekNumberEnd; i++)
            {
                var thisWeekShifts = ShiftsInRota.Where(s => s.WeekNumber == i);
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

        private static bool Max72Per168()
        {

            Console.WriteLine("Checking max 72 hours per 168 hour period...");

            for (int i = 0; i < Rota.Length.Days; i++)
            {
                //Get midnight on the first day to be checked
                DateTime setMidnight = new DateTime(Rota.RotaStartTime.Year, Rota.RotaStartTime.Month, Rota.RotaStartTime.Day);

                //Select the next date to be cycled through and add 7 days
                DateTime startDateTime = setMidnight.AddDays(i);
                DateTime endDateTime = startDateTime.AddDays(7);

                //If there are less than 7 days remaining, then there is no need to check further
                if (DateTime.Compare(Rota.RotaEndTime.AddDays(1), endDateTime) < 0)
                {
                    break;
                }

                double thisWeeklyHours = 0;
                Console.WriteLine($"Checking {startDateTime} to {endDateTime}");

                //Selects all the shifts with start or end time within window
                var thisWeekShifts = ShiftsInRota.Where(y => (DateTime.Compare(startDateTime, y.StartTime) <= 0 && DateTime.Compare(endDateTime, y.StartTime) > 0) || (DateTime.Compare(startDateTime, y.EndTime) < 0 && DateTime.Compare(endDateTime, y.EndTime) >= 0));

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

        private static bool Max13HourShift()
        {
            Console.WriteLine("Checking max 13 hours per shift...");

            foreach (Shift s in ShiftsInRota)
            {
                if (s.Length.TotalHours > 13)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool Max4LongShifts()
        {

            Console.WriteLine("Checking max 4 long shifts consecutively...");

            //Cycle through all shifts
            for (int i = 0; i < ShiftsInRota.Count() - 1; i++)
            {
                //If a long shift is found
                if (ShiftsInRota[i].Long)
                {
                    //If there are 4 more shifts after
                    if (ShiftsInRota.Count >= i + 5)
                    {

                        List<Shift> setOfFive = ShiftsInRota.GetRange(i, 5);

                        //Check if consecutive

                        DateTime day1 = new DateTime(setOfFive[0].StartTime.Year, setOfFive[0].StartTime.Month, setOfFive[0].StartTime.Day);
                        DateTime day4 = new DateTime(setOfFive[3].StartTime.Year, setOfFive[3].StartTime.Month, setOfFive[3].StartTime.Day);
                        DateTime day5 = new DateTime(setOfFive[4].StartTime.Year, setOfFive[4].StartTime.Month, setOfFive[4].StartTime.Day);

                        if (DateTime.Compare(day1.AddDays(3), day4) >= 0)
                        {
                            //5 in a row returns fail test
                            if(DateTime.Compare(day1.AddDays(4), day5) >= 0)
                            {
                                if (ShiftsInRota[i].Long && ShiftsInRota[i + 1].Long && ShiftsInRota[i + 2].Long && ShiftsInRota[i + 3].Long && ShiftsInRota[i + 4].Long)
                                {
                                    return false;
                                }
                            }
                            
                            //4 in a row will check whether breaks are adhered to
                            if (ShiftsInRota[i].Long && ShiftsInRota[i + 1].Long && ShiftsInRota[i + 2].Long && ShiftsInRota[i + 3].Long)
                            {

                                //Check if break will be after 4th or 5th shift

                                bool noEvenings = (!ShiftsInRota[i].EveningFinish && !ShiftsInRota[i + 1].EveningFinish && !ShiftsInRota[i + 2].EveningFinish && !ShiftsInRota[i + 3].EveningFinish);
                                bool noNights = (!ShiftsInRota[i].Night && !ShiftsInRota[i + 1].Night && !ShiftsInRota[i + 2].Night && !ShiftsInRota[i + 3].Night);

                                if (noEvenings && noNights)
                                {
                                    if (ShiftsInRota.Count >= i + 6)
                                    {
                                        //Can work another
                                        List<Shift> setOfSix = ShiftsInRota.GetRange(i, 6);

                                        //Check if 5th is consecutive
                                        DateTime day1b = new DateTime(setOfSix[0].StartTime.Year, setOfSix[0].StartTime.Month, setOfSix[0].StartTime.Day);
                                        DateTime day5b = new DateTime(setOfSix[4].StartTime.Year, setOfSix[4].StartTime.Month, setOfSix[4].StartTime.Day);

                                        if (DateTime.Compare(day1b.AddDays(4), day5b) >= 0)
                                        {
                                            Console.WriteLine($"Break Required after {ShiftsInRota[i + 4].EndTime} - {CheckBreakRule(setOfSix)}");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Break Required after {ShiftsInRota[i + 4].EndTime}");
                                        }

                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Break Required after {ShiftsInRota[i + 3].EndTime} - {CheckBreakRule(setOfFive)}");
                                }
                            }
                        }

                    }

                }
            }
            return true;
        }

        private static bool CheckBreakRule(List<Shift> shifts)
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

        private static bool Max7ConsecutiveDays()
        {
            //For shift in the rota
            //Check if the next 8 days are worked

            Console.WriteLine("Checking max 7 days consecutively...");

            //Cycle through all shifts
            for (int i = 0; i < ShiftsInRota.Count - 1; i++)
            {
                //If there are 7 more shifts after
                if (ShiftsInRota.Count >= i + 8)
                {
                    List<Shift> setOfEight = ShiftsInRota.GetRange(i, 8);

                    //Check if consecutive

                    DateTime day1 = new DateTime(setOfEight[0].StartTime.Year, setOfEight[0].StartTime.Month, setOfEight[0].StartTime.Day);
                    DateTime day7 = new DateTime(setOfEight[6].StartTime.Year, setOfEight[6].StartTime.Month, setOfEight[6].StartTime.Day);
                    DateTime day8 = new DateTime(setOfEight[7].StartTime.Year, setOfEight[7].StartTime.Month, setOfEight[7].StartTime.Day);

                    if (DateTime.Compare(day1.AddDays(7), day8) >= 0)
                    {
                        Console.WriteLine($"8 consecutive shifts after {ShiftsInRota[i].StartTime}");

                        return false;
                        
                    }

                    if (DateTime.Compare(day1.AddDays(6), day7) >= 0)
                    {
                        Console.WriteLine($"48 hours break required after {ShiftsInRota[i+6].EndTime}");

                        TimeSpan gap = ShiftsInRota[i+7].StartTime - ShiftsInRota[i + 6].StartTime;

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

        private static bool AtLeast11HoursRest()
        {
            Console.WriteLine("Checking 11 hours rest between shifts...");

            //Cycle through all shifts up to the last
            for (int i = 0; i < ShiftsInRota.Count - 2; i++)
            {
                TimeSpan gap = ShiftsInRota[i + 1].StartTime - ShiftsInRota[i].EndTime;

                if(gap.TotalHours < 11)
                {
                    Console.WriteLine($"Less than 11 hours between {ShiftsInRota[i].EndTime} and {ShiftsInRota[i + 1].StartTime}");
                    return false;
                }
                    
            }
            return true;
        }

        private static bool NightRestBreaks()
        {
            Console.WriteLine("Checking every set of night shifts has a 46 hour break...");
            
            for(int i = 0; i < ShiftsInRota.Count - 1; i++)
            {
                if (ShiftsInRota[i].Night)
                {
                    //Check the next one is not also a night shift
                    if (!ShiftsInRota[i + 1].Night)
                    {
                        TimeSpan gap = ShiftsInRota[i + 1].StartTime - ShiftsInRota[i].EndTime;

                        if (gap.TotalHours < 46)
                        {
                            Console.WriteLine($"Less than 46 hours rest after {ShiftsInRota[i].EndTime}");
                            return false;
                        }
                    }

                }
            }
            return true;
        }

        private static bool WeekendFrequency()
        {
            Console.WriteLine("Checking no more than 1 in 2 weekends...");
            
            List<int> weekendWork = new List<int>();

            for (int i = Rota.WeekNumberStart; i <= Rota.WeekNumberEnd; i++)
            {
                var thisWeekDuties = Rota.Duties.Where(s => s.WeekNumber == i);
                if (thisWeekDuties.Any(s => s.Weekend))
                {
                    weekendWork.Add(1);
                }
                else
                {
                    weekendWork.Add(0);
                }                
            }

            if(weekendWork.Average() > 0.5)
            {
                Console.WriteLine("More than 1 in 2 weekends worked");
                return false;
            }
            else
            {
                return true;
            }

        }

        private static bool Max24HourOnCall()
        {
            Console.WriteLine("Checking max 24 hours on call period...");

            foreach (OnCallPeriod o in OnCallInRota)
            {
                if (o.Length.TotalHours > 24)
                {
                    return false;
                }
            }

            return true;
        }

        //No consecutive on-call periods other than Saturday and Sunday. No more than 3 on-call periods in 7 consecutive days

        //Day after an on-call period must not have work rostered longer 10 hours
        //Where Saturday and Sunday on-calls are worked consecutively, this rule applies to the day after the Sunday on-call

        //Expected rest of 8 hours per 24 hour on-call period, 5 hours must be continuous between 22:00-07:00



    }
}
