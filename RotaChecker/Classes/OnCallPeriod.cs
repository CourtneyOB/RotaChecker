using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker
{
    public class OnCallPeriod : WorkDuty
    {
        public TimeSpan ExpectedHours { get; }

        public OnCallPeriod(DateTime start, DateTime end, TimeSpan expectedHours) : base(start, end)
        {
            ExpectedHours = expectedHours;

        }

    }

}
