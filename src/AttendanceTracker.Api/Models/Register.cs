using System.ComponentModel.DataAnnotations;

namespace AttendanceTracker.Api.Models;

public class Register
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public bool IsAdmin { get; set; }
    [Required]
    public bool IsManager { get; set; }
    [Required]
    public bool IsEmployee { get; set; }

    public string Role
    {
        get
        {
            if (IsAdmin) return "admin";
            if (IsManager) return "manager";
            if (IsEmployee) return "employee";
            return null;
        }
    }
}