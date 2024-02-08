using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using eKoncert.Data;
using eKoncert.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Security.Claims;

namespace eKoncert.Pages
{
    public class ConcertDetailsModel : PageModel
    {
        private readonly EventDbContext _eventDbContext;
        private readonly TicketsDbContext _ticketsDbContext;
		public bool HasError { get; set; }
		public string ErrorMessage { get; set; }
		public ConcertDetailsModel(EventDbContext eventDbContext, TicketsDbContext ticketsDbContext)
        {
            _eventDbContext = eventDbContext;
            _ticketsDbContext = ticketsDbContext;
        }

        public Event Event { get; set; }

        public IActionResult OnGet(int id)
        {
            Event = _eventDbContext.Events.Find(id);

            if (Event == null)            {
                return NotFound();
            }

            return Page();
        }

        
        [BindProperty]
        public int EventId { get; set; }

        [BindProperty]
        public string UserId { get; set; }

        public Tickets Tickets { get; set; }

        public IActionResult OnPost()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var @event = _eventDbContext.Events.Find(EventId);
                    if (@event != null && @event.TicketsRemaining >= 1)
                    {
                        @event.TicketsRemaining--;

                        _eventDbContext.SaveChanges();

                        var newTicket = new Tickets
                        {
                            EventId = EventId,
                            UserId = userId.ToString(),
                        };

                        _ticketsDbContext.Tickets.Add(newTicket);
                        _ticketsDbContext.SaveChanges();

                        return RedirectToPage("/Events");
                    }
                    else if (@event == null)
                    {
                        // Handle the case where the event is not found
                        return NotFound();
                    }
                    else
                    {
                        HasError = true;
                        ErrorMessage = "No more tickets available for this event.";
                    }
                }
                else
                {
                    return BadRequest("User ID not found in claims. Bad Cookie");
                }
            

            // If the code reaches here, it means the event is not null but tickets are not available
            Event = _eventDbContext.Events.Find(EventId);
            return Page();
        }


    }
}
