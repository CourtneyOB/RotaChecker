using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker.Classes
{
    public abstract class Template
    {
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public double Length { get; set; }

        public Template(string name, TimeSpan start, double length)
        {
            Name = name;
            StartTime = start;
            Length = length;
        }
    }
}
