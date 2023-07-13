using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AttendanceTracker.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AttendanceTracker.Core.Entities.Account;

public class Role : IdentityRole<string>
{

    public List<ModulesAccess> ModulesAccesses { get; set; }

}