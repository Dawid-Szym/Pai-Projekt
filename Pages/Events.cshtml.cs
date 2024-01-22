using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;  // Import this for async operations if needed
using eKoncert.Models;  // Import the correct namespace for your models

namespace eKoncert.Pages
{
    public class EventsModel : PageModel
    {
        private readonly ILogger<EventsModel> _logger;
        private readonly EventDbContext _dbContext;

        public EventsModel(ILogger<EventsModel> logger, EventDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public List<Event> Events { get; set; }

        public async Task OnGetAsync()
        {
            Events = await _dbContext.Events.ToListAsync();
            _logger.LogInformation($"Retrieved {Events.Count} events from the database.");
        }

    }
}
