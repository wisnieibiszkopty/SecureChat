using System.ComponentModel.DataAnnotations;

namespace MFAWebApp.Models.Authentication;

public class LoginInputDto
{
    [Required(ErrorMessage = "Email required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }
}