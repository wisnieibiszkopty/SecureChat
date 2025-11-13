using MFAWebApp.Data;
using MFAWebApp.Models;
using MFAWebApp.Models.Authentication;
using MFAWebApp.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MFAWebApp.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterModel(AppDbContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    [BindProperty]
    public RegisterInputDto Input { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        bool emailExists = _db.Users.Any(u => u.Email == Input.Email);
        if (emailExists)
        {
            ModelState.AddModelError(string.Empty, "User with this email already exists");
            return Page();
        }

        var user = new User
        {
            Email = Input.Email,
            PasswordHash = _passwordHasher.Hash(Input.Password),
            MfaEnabled = false,
            TotpSecret = null
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return RedirectToPage("/Account/Login");
    }
}
