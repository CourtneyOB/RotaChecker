using System;
using System.Collections.Generic;
using System.Text;
using RotaChecker.Classes;

namespace RotaChecker
{
    public class Session
    {

        public Rota CurrentRota { get; }
        public Month CurrentMonth { get; set; }
        public List<DayEvents> DutiesInMonth { get; set; } = new List<DayEvents>();
        public Session()
        {
            CurrentRota = RotaBuilder.CreateRota();

            CurrentMonth = new Month(CurrentRota.RotaStartTime);

        }

        public void GetDutiesInCurrentMonth()
        {
            foreach (DateTime date in CurrentMonth.DaysInMonth)
            {
                DayEvents day = new DayEvents(date);
                day.Duties = CurrentRota.GetDutiesOnDate(day.Date);
            }
        }

    }
}
