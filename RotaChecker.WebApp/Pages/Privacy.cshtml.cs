using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotaChecker.WebApp.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        [BindProperty]
        public string DatesFromDatePicker { get; set; }
        public DateTime TestDateTime { get; set; }
        public List<DateTime> Times { get; set; } = new List<DateTime>();

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            List<string> dateList = DatesFromDatePicker.Split(',').ToList();
            foreach(var date in dateList)
            {
                Times.Add(DateTime.Parse(date));
            }
        }
    }
}
