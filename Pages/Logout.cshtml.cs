using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eKoncert.Pages
{
    public class LogoutModel : PageModel
    {
       public IActionResult OnGet()
		{
			HttpContext.Response.Cookies.Delete("token", new CookieOptions { Secure = true, HttpOnly = true, SameSite = SameSiteMode.None });
			return RedirectToPage("/Index");
		}
	}
}
