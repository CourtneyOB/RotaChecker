﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker.Classes
{
    public class OnCallPeriod : WorkDuty
    {
        public TimeSpan ExpectedHours { get; }

        public OnCallPeriod(DateTime start, DateTime end, TimeSpan expectedHours, string templateName = null) : base(start, end, templateName)
        {
            ExpectedHours = expectedHours;

        }

    }

}
