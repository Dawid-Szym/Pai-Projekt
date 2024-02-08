using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using eKoncert.Data;
using eKoncert.Models;
using System.Security.Claims;

namespace ekoncert.Pages
{
    public class EditModel : PageModel
    {
		private readonly EventDbContext _context;
		private readonly IWebHostEnvironment _environment;
		[BindProperty]
        public Event Event { get; set; }
        [BindProperty]
        public IFormFile ImageFile { get; set; }
        public EditModel(EventDbContext context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}

		public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Event = await _context.Events.FindAsync(id);

            if (Event == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var modelStateEntry = ModelState[key];
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Console.WriteLine($"Property: {key}, Error: {error.ErrorMessage}");
                    }
                }

                return Page();
            }



            var userIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToPage("/Error");
            }
            Event.CreatedBy = userId;

			if (ImageFile != null)
			{
				var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
				var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
				var filePath = Path.Combine(uploadsFolder, uniqueFileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await ImageFile.CopyToAsync(fileStream);
				}

				Event.Url = Path.Combine("/images/", uniqueFileName);
			}
            else
            {
                Event.Url = Event.Url;
            }






            _context.Attach(Event).State = EntityState.Modified;
            // Log EntityState for debugging
            Console.WriteLine($"EntityState after attach: {_context.Entry(Event).State}");

            try
            {
                await _context.SaveChangesAsync();
                // Log EntityState after SaveChangesAsync
                Console.WriteLine($"EntityState after SaveChangesAsync: {_context.Entry(Event).State}");
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflict
                if (!EventExists(Event.Id))
                {
                    return RedirectToPage("/Manage"); ;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Another user has modified the data. Please try again.");
                    return RedirectToPage("/Manage");
                }
            }

            // Redirect to Manage page after successful update
            return RedirectToPage("/Manage");
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
