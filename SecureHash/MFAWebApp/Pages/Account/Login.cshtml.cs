using MFAWebApp.Data;
using MFAWebApp.Models.Authentication;
using MFAWebApp.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher _passwordHasher;

        public LoginModel(AppDbContext db, IPasswordHasher passwordHasher)
        {
            _db = db;
            _passwordHasher = passwordHasher;
        }

        [BindProperty]
        public LoginInputDto Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == Input.Email);

            if (user == null || !_passwordHasher.Verify(Input.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return Page();
            }

            if (user.MfaEnabled)
            {
                HttpContext.Session.SetInt32("PendingUserId", user.Id);
                return RedirectToPage("/Account/VerifyMfa");
            }

            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToPage("/Index");
        }
    }
}
