using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using eKoncert.Models;
using eKoncert.Data;
using System;
using System.IO;
using eKoncert.Pages;
using Microsoft.Extensions.Logging;

namespace ekoncert.Pages
{
    public class AddEventModel : PageModel
    {
        private readonly EventDbContext _context;
        private readonly ILogger<EventsModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        [BindProperty]
        public Event Event { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public AddEventModel(EventDbContext context, ILogger<EventsModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnPost()
        {
            string userIdString = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToPage("/Error");
            }

            Event.CreatedBy = userId;

            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var modelStateEntry = ModelState[key];
                    if (modelStateEntry.Errors.Any())
                    {
                        foreach (var error in modelStateEntry.Errors)
                        {
                            _logger.LogInformation($"ModelState Error for {key}: {error.ErrorMessage}");
                        }
                    }
                }
                return Page();
            }
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                var filePath = Path.Combine("wwwroot", "images", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }

                TempData["ImageFilePath"] = filePath;
                Event.Url = "/images/" + uniqueFileName;

            }
            _context.Events.Add(Event);

            _context.SaveChanges();

            return RedirectToPage("/Manage");
        }
    }
}
