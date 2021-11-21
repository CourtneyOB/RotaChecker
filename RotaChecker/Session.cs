using System;
using System.Collections.Generic;
using System.Text;
using RotaChecker.Classes;

namespace RotaChecker
{
    public class Session
    {
        public Rota CurrentRota { get; }
        public List<ShiftTemplate> Templates { get; set; }
        public Session()
        {
            CurrentRota = RotaBuilder.CreateRota();
            Templates = new List<ShiftTemplate>();
            ShiftTemplate normalWorkingDay = new ShiftTemplate("Normal Working Day", 8.0);
            Templates.Add(normalWorkingDay);

            CurrentRota.AddShiftTemplate(Templates[0], new DateTime(2021, 11, 4, 9, 0, 0));
            CurrentRota.AddShiftTemplate(Templates[0], new DateTime(2021, 11, 5, 9, 0, 0));
            CurrentRota.AddShiftTemplate(Templates[0], new DateTime(2021, 11, 6, 9, 0, 0));
            CurrentRota.AddShiftTemplate(Templates[0], new DateTime(2021, 11, 7, 9, 0, 0));
            CurrentRota.AddShiftTemplate(Templates[0], new DateTime(2021, 11, 8, 9, 0, 0));

        }

    }
}
