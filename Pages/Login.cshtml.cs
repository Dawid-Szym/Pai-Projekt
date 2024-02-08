using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using eKoncert.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System;

namespace eKoncert.Pages.Shared
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly UserDbContext _userDbContext;

        public LoginModel(UserDbContext userDbContext, IConfiguration configuration)
        {
            _userDbContext = userDbContext;
            _configuration = configuration;
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
               
                GenerateJwtToken(user);
                return RedirectToPage("/Events");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return Page();
            }
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.AccountType.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "ekoncert",
                audience: "users",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);



            HttpContext.Response.Cookies.Append("token", tokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });


            return tokenString;
        }
    }
}
