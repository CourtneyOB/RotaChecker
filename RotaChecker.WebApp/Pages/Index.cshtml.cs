using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RotaChecker.WebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RotaChecker.Classes;

namespace RotaChecker.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public RotaRetrieverService RotaRetrieverService;
        public DateTime StartTime;
        public DateTime EndTime;
        public List<WorkDuty> WorkDuties { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, RotaRetrieverService rotaRetrieverService)
        {
            _logger = logger;
            RotaRetrieverService = rotaRetrieverService;
        }

        public void OnGet()
        {
            WorkDuties = RotaRetrieverService.GetWorkDuties();
        }

        public void OnPost()
        {
            RotaRetrieverService.AddShift(new Shift(new DateTime(2021, 12, 1, 9, 0, 0), new DateTime(2021, 12, 1, 17, 0, 0)));
            OnGet();
        }
    }
}
