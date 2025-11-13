using System.ComponentModel.DataAnnotations;

namespace MFAWebApp.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    public string? TotpSecret { get; set; }
    public bool MfaEnabled { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}