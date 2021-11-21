using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RotaChecker.Classes;

namespace RotaChecker.WebApp.Services
{
    public class RotaRetrieverService
    {
        private Session _session = new Session();
        public List<ShiftTemplate> Templates = new List<ShiftTemplate>();
        public List<WorkDuty> GetWorkDuties()
        {
            return _session.CurrentRota.Duties;
        }

        public void AddTemplate(ShiftTemplate template)
        {
            Templates.Add(template);
        }

        public void AddShift(Shift shift)
        {
            _session.CurrentRota.AddShift(shift);
        }
    }
}
