using System.Collections.Generic;
using System.Security.Claims;
using eKoncert.Data;
using eKoncert.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace eKoncert.Pages
{
	[Authorize]
	public class TicketsModel : PageModel
	{
		private readonly TicketsDbContext _ticketsDbContext;
		private readonly UserDbContext _userDbContext;
		private readonly EventDbContext _eventDbContext;

		public TicketsModel(TicketsDbContext ticketsDbContext, UserDbContext userDbContext, EventDbContext eventDbContext)
		{
			_ticketsDbContext = ticketsDbContext;
			_userDbContext = userDbContext;
			_eventDbContext = eventDbContext;
		}

		public List<TicketInfo> TicketList { get; set; } = new List<TicketInfo>();

		public class TicketInfo
		{
			public int Id { get; set; }
			public int EventId { get; set; }
			public string UserId { get; set; }
			public string ConcertName { get; set; }
			public DateOnly? StartDate { get; set; }
			public DateOnly? EndDate { get; set; }
			public string ConcertImageUrl { get; set; }
		}


		public void LoadTicketList()
		{
			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

			if (userIdClaim != null)
			{
				var userId = userIdClaim.Value;

				TicketList = _ticketsDbContext.Tickets
					.Where(ticket => ticket.UserId == userId)
					.Select(ticket => new TicketInfo
					{
						Id = ticket.Id,
						EventId = ticket.EventId,
						UserId = ticket.UserId,
					})
					.ToList();

				foreach (var ticketInfo in TicketList)
				{
					var @event = _eventDbContext.Events.Find(ticketInfo.EventId);
					if (@event != null)
					{
						ticketInfo.ConcertName = @event.Name;
						ticketInfo.StartDate = @event.DateStart;
						ticketInfo.EndDate = @event.DateEnd;
						ticketInfo.ConcertImageUrl = @event.Url;
					}
				}
			}
		}

		public void OnGet()
		{
			LoadTicketList();
		}

		public IActionResult OnPostRemoveTicket(int ticketId)
		{
			var ticketToRemove = _ticketsDbContext.Tickets.FirstOrDefault(t => t.Id == ticketId);

			if (ticketToRemove != null)
			{
				var eventId = ticketToRemove.EventId;

				_ticketsDbContext.Tickets.Remove(ticketToRemove);
				_ticketsDbContext.SaveChanges();

				IncrementRemainingTickets(eventId);
			}


			return RedirectToPage("/Tickets");
		}
		private void IncrementRemainingTickets(int eventId)
		{
			// Find the event in the Events database by its ID
			var eventToUpdate = _eventDbContext.Events.FirstOrDefault(e => e.Id == eventId);

			if (eventToUpdate != null)
			{
				// Increment the remaining tickets count for the event
				eventToUpdate.TicketsRemaining++;
				_eventDbContext.SaveChanges();
			}
		}


	}
}
