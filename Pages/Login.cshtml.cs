using eKoncert.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eKoncert.Pages.Shared
{
    public class LoginModel : PageModel
    {
        private readonly UserDbContext _userDbContext;

        public LoginModel(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public void OnGet()
        {
        }


        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnPost()
        {
            var user = _userDbContext.Users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (user != null)
            {
                // User authentication successful

                return RedirectToPage("/Events");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return Page();
            }
        }

    }
}
