using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker.Classes
{
    public class OnCallPeriod : WorkDuty
    {
        public TimeSpan ExpectedHours { get; }

        public OnCallPeriod(DateTime start, DateTime end, TimeSpan expectedHours, int weekYearDifference, string templateName = null) : base(start, end, weekYearDifference, templateName)
        {
            ExpectedHours = expectedHours;

        }

    }

}
