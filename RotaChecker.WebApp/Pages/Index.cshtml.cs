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
        public List<Template> Templates { get; private set; }
        [BindProperty]
        public string Type { get; set; }
        [BindProperty]
        public string TemplateName { get; set; }
        [BindProperty]
        public double Length { get; set; }
        [BindProperty]
        public double ExpectedHours { get; set; }

        public IndexModel(ILogger<IndexModel> logger, RotaRetrieverService rotaRetrieverService)
        {
            _logger = logger;
            RotaRetrieverService = rotaRetrieverService;
        }

        public void OnGet()
        {
            Templates = RotaRetrieverService.GetTemplates();
        }

        public void OnPost()
        {
            if(Type == "shift")
            {
                RotaRetrieverService.AddTemplate(new ShiftTemplate(TemplateName, Length));
            }
            if(Type == "onCall")
            {
                RotaRetrieverService.AddTemplate(new OnCallTemplate(TemplateName, Length, ExpectedHours));
            }

            Templates = RotaRetrieverService.GetTemplates();
        }
    }
}
