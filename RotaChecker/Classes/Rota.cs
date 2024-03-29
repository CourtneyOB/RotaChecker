﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

namespace RotaChecker.Classes
{
    public class Rota : CalendarBase
    {
        public List<WorkDuty> Duties { get; set; }
        public DateTime RotaStartTime { get; set; }
        public DateTime RotaEndTime { get; set; }
        public TimeSpan Length { get; private set; }
        public int WeekNumberStart { get; private set; }
        public int WeekNumberEnd { get; private set; }

        //Methods

        public Rota()
        {
            Duties = new List<WorkDuty>();
            RotaStartTime = DateTime.Now;
        }        

        public List<string> Describe()
        {
            List<string> response = new List<string>();
            foreach (WorkDuty d in Duties)
            {
                if (d is Shift s)
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

                else if (d is OnCallPeriod o)
                {
                    response.Add($"The on call starts at {o.StartTime} and ends at {o.EndTime}.\nThe expected work hours of the period are {o.ExpectedHours.TotalHours} hours.");
                    response.Add($"Week number is {o.WeekNumber}");
                    if (o.Weekend)
                    {
                        response.Add($"It is a weekend on call");
                    }
                    else
                    {
                        response.Add($"It is not a weekend on call");
                    }
                }
            }
            return response;
        }

        public void CanAddShift(Shift s)
        {
            if (DateTime.Compare(s.StartTime, s.EndTime) >= 0)
            {
                throw new ArgumentException("Start time must be before end time");
            }

            List<Shift> shiftsInRota = GetShifts();

            foreach (Shift shift in shiftsInRota)
            {
                if (shift.StartTime <= s.StartTime && s.StartTime < shift.EndTime)
                {
                    throw new ArgumentException("New shift overlaps with existing shift");
                }
                else if (shift.EndTime >= s.EndTime && s.EndTime > shift.StartTime)
                {
                    throw new ArgumentException("New shift overlaps with existing shift");
                }
                else if (shift.StartTime >= s.StartTime && s.EndTime >= shift.EndTime)
                {
                    throw new ArgumentException("New shift overlaps with existing shift");
                }
            }
        }
        public void CanAddOnCall(OnCallPeriod o)
        {
            if (DateTime.Compare(o.StartTime, o.EndTime) >= 0)
            {
                throw new ArgumentException("Start time must be before end time");
            }

            if (o.ExpectedHours.TotalHours <= 0)
            {
                throw new ArgumentException("Expected hours must be more than 0");
            }

            List<OnCallPeriod> onCallsInRota = GetOnCalls();

            foreach (OnCallPeriod onCall in onCallsInRota)
            {
                if (onCall.StartTime <= o.StartTime && o.StartTime < onCall.EndTime)
                {
                    throw new ArgumentException("New on call overlaps with existing on call");
                }
                else if (onCall.EndTime >= o.EndTime && o.EndTime > onCall.StartTime)
                {
                    throw new ArgumentException("New on call overlaps with existing on call");
                }
                else if (onCall.StartTime >= o.StartTime && o.EndTime >= onCall.EndTime)
                {
                    throw new ArgumentException("New on call overlaps with existing shift");
                }
            }
        }
        public void AddShift(Shift s)
        {

            CanAddShift(s);
                
            Duties.Add(s);
            Duties = Duties.OrderBy(d => d.StartTime).ToList();
            RotaStartTime = Duties.Select(d => d.StartTime).Min(); 
            RotaEndTime = Duties.Select(d => d.EndTime).Max(); 
            Length = RotaEndTime - RotaStartTime; 
            WeekNumberStart = Calendar.GetWeekOfYear(RotaStartTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            WeekNumberEnd = Calendar.GetWeekOfYear(RotaEndTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);


        }
        public void AddOnCall(OnCallPeriod o)
        {

            CanAddOnCall(o);


            Duties.Add(o);
            Duties = Duties.OrderBy(d => d.StartTime).ToList();
            RotaStartTime = Duties.Select(d => d.StartTime).Min();
            RotaEndTime = Duties.Select(d => d.EndTime).Max();
            Length = RotaEndTime - RotaStartTime;

            WeekNumberStart = Calendar.GetWeekOfYear(RotaStartTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            WeekNumberEnd = Calendar.GetWeekOfYear(RotaEndTime, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        }

        public void AddTemplateToDateList(Template t, List<DateTime> dates)
        {
            foreach (DateTime d in dates)
            {
                if (t is ShiftTemplate)
                {
                    DateTime startTime = new DateTime(d.Year, d.Month, d.Day, t.StartTime.Hours, t.StartTime.Minutes, 0);
                    DateTime endTime = startTime.AddHours(t.Length);
                    AddShift(new Shift(startTime, endTime, t.Name));
                }
                if (t is OnCallTemplate)
                {
                    object o = t.GetType().GetProperty("ExpectedHours")?.GetValue(t, null);
                    double expectedHours = (double)o;

                    DateTime startTime = new DateTime(d.Year, d.Month, d.Day, t.StartTime.Hours, t.StartTime.Minutes, 0);
                    DateTime endTime = startTime.AddHours(t.Length);
                    AddOnCall(new OnCallPeriod(startTime, endTime, TimeSpan.FromHours(expectedHours), t.Name));
                }
            }

        }

        public List<Shift> GetShifts()
        {
            List<Shift> shiftsInRota = Duties.OfType<Shift>().ToList();
            return shiftsInRota;
        }

        public List<OnCallPeriod> GetOnCalls()
        {
            List<OnCallPeriod> onCallInRota = Duties.OfType<OnCallPeriod>().ToList();
            return onCallInRota;
        }
        
        public List<WorkDuty> GetDutiesOnDate(DateTime date)
        {
            return Duties.Where(d => d.StartTime.Date == date.Date).ToList();
        }

    }
}
