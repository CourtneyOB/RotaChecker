﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RotaChecker.Classes;

namespace RotaChecker.WebApp.Services
{
    public class RotaRetrieverService
    {
        private Session _session = new Session();
        private List<Template> _templates = new List<Template>();
        public List<WorkDuty> GetWorkDuties()
        {
            return _session.CurrentRota.Duties;
        }

        public List<Template> GetTemplates()
        {
            return _templates;
        }

        public void AddTemplate(Template template)
        {
            _templates.Add(template);
        }

        public void AddShift(Shift shift)
        {
            _session.CurrentRota.AddShift(shift);
        }
    }
}
