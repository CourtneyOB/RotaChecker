using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker.Classes
{
    public class OnCallTemplate : Template
    {
        public double ExpectedHours { get; set; }
        public OnCallTemplate(string name, double length, double expectedHours): base(name, length)
        {
            ExpectedHours = expectedHours;
        }
    }
}
