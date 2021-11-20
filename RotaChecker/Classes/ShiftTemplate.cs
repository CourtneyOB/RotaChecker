using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker.Classes
{
    public class ShiftTemplate
    {
        public string Name { get; set; }
        public double Length { get; set; }
        private Rota Rota { get; }

        public ShiftTemplate(Rota rota, string name, double length)
        {
            Rota = rota;
            Name = name;
            Length = length;
        }

        public void CreateShift(DateTime startTime)
        {
            DateTime endTime = startTime.AddHours(Length);
            Rota.AddShift(new Shift(startTime, endTime));
        }

    }
}
