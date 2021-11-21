using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker.Classes
{
    public class ShiftTemplate
    {
        public string Name { get; set; }
        public double Length { get; set; }

        public ShiftTemplate(string name, double length)
        {
            Name = name;
            Length = length;
        }


    }
}
