using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker.Classes
{
    public abstract class Template
    {
        public string Name { get; set; }
        public double Length { get; set; }

        public Template(string name, double length)
        {
            Name = name;
            Length = length;
        }
    }
}
