using eKoncert.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace eKoncert.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserDbContext _userDbContext;


        [BindProperty]
        public User NewUser { get; set; }







        public RegisterModel(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public void OnGet()
        {
        }


        public IActionResult OnPost()
        {
            _userDbContext.Users.Add(NewUser);


            //Always save changes
            _userDbContext.SaveChanges();


            return RedirectToPage("/Login");
        }


    }
}
