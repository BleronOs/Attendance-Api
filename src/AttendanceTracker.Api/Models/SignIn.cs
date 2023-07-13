using System.ComponentModel.DataAnnotations;

namespace AttendanceTracker.Api.Models;

public class SignIn
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}