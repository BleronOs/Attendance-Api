using AttendanceTracker.Api.Authentication;
using AttendanceTracker.Core.Entities.Account;

namespace AttendanceTracker.Api.ViewModels;

public class SignInSucceededViewModel
{
    public User User { get; set; }
    public JwtToken Token { get; set; }
    public Role Roles { get; set; }
    public List<Core.ResultModels.RoleModule> RoleModules { get; set; }
    public List<Core.ResultModels.ResultUser> UserId { get; set; }
}
