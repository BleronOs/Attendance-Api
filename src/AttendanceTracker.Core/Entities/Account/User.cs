using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AttendanceTracker.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AttendanceTracker.Core.Entities.Account;

public class User : IdentityUser<string>, IEntity
{
    [MaxLength(100)]
    [Column("first_name")]
    public string FirstName { get; set; }
    [MaxLength(100)]
    [Column("last_name")]
    public string LastName { get; set; }
    [Column("modify_date")]
    public DateTime ModifyDate { get; set; }
    [Column("active")]
    public bool IsActive { get; set; }
    [Column("registration_date")]
    public DateTime RegistrationDate { get; set; }
}