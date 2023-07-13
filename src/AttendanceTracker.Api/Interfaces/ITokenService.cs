using AttendanceTracker.Api.Authentication;

namespace AttendanceTracker.Api.Interfaces;

public interface ITokenService
{
    JwtToken GenerateToken(string userId, string role);
}
