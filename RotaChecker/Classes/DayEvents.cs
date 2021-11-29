using System;
using System.Collections.Generic;
using System.Text;

namespace RotaChecker.Classes
{
    public class DayEvents
    {
        public DateTime Date { get; }
        public List<WorkDuty> Duties { get; set; } = new List<WorkDuty>();

        public DayEvents(DateTime date)
        {
            Date = date;
        }
    }
}
