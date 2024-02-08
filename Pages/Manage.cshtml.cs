using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using eKoncert.Models;
using eKoncert.Data;

namespace ekoncert.Pages
{
    public class ManageModel : PageModel
    {
        private readonly EventDbContext _context;

        public ManageModel(EventDbContext context)
        {
            _context = context;
        }

        public List<Event> Events { get; set; }

        public void OnGet()
        {
            Events = _context.Events.ToList();
        }
    }
}
