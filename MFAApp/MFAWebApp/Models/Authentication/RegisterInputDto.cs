using System.ComponentModel.DataAnnotations;

namespace MFAWebApp.Models.Authentication;

public class RegisterInputDto
{
    [Required(ErrorMessage = "Email required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password should be between 8 and 100 characters")]
    [RegularExpression(
        @"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>/?]).+$",
        ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character")
    ]
    [Display(Name = "Password")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Password required")]
    [Compare("Password")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password should be between 8 and 100 characters")]
    [RegularExpression(
        @"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>/?]).+$",
        ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character")
    ]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; }
}