using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace RotaChecker.Classes
{
    public class TemplateLibrary : Notifications
    {
        public ObservableCollection<Template> TemplateList { get; private set; }
        public List<Template> ShiftTemplates => TemplateList.Where(i => i is ShiftTemplate).ToList();
        public List<Template> OnCallTemplates => TemplateList.Where(i => i is OnCallTemplate).ToList();

        public TemplateLibrary()
        {
            TemplateList = new ObservableCollection<Template>();
            AddTemplate(new ShiftTemplate("Normal day", new TimeSpan(9,0,0), 8.0));
            AddTemplate(new OnCallTemplate("On call weekend", new TimeSpan(9,0,0), 24.0, 8.0));
            AddTemplate(new OnCallTemplate("On call evening", new TimeSpan(16, 0, 0), 17.0, 4.0));
            AddTemplate(new OnCallTemplate("Friday on call", new TimeSpan(13, 0, 0), 20.0, 4.0));
        }

        public void AddTemplate(ShiftTemplate template)
        {
            TemplateList.Add(template);
            OnPropertyChanged(nameof(ShiftTemplates));
        }

        public void AddTemplate(OnCallTemplate template)
        {
            TemplateList.Add(template);
            OnPropertyChanged(nameof(OnCallTemplates));
        }

    }
}
