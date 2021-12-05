using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker.Classes
{
    public class OnCallTemplate : Template
    {
        public double ExpectedHours { get; set; }
        public OnCallTemplate(string name, TimeSpan start, double length, double expectedHours): base(name, start, length)
        {
            ExpectedHours = expectedHours;
        }
    }
}
