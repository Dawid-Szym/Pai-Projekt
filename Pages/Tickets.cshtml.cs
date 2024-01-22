using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eKoncert.Pages
{
    public class TicketsModel : PageModel
    {
        private readonly ILogger<TicketsModel> _logger;

        public TicketsModel(ILogger<TicketsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }

}
