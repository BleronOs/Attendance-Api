using System.Security.Claims;
using AttendanceTracker.Api.Constants;
using AttendanceTracker.Core.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Api;

public class ExtendedBaseController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    public ExtendedBaseController(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    public string AuthenticatedUserEmail => (string)HttpContext.Items["User"];
    
    public string AuthenticatedUserId => GetClaimValueFromContext(ClaimTypeConstants.USER_ID);

    private string GetValueFromHeaders(string type)
    {
        HttpContext.Request.Headers.TryGetValue(type, out var value);
        return value;
    }

    [NonAction]
    public async Task<User> GetAuthenticatedUserAsync()
    {
        var email = (string)HttpContext.Items["User"];
        if (string.IsNullOrEmpty(email)) return null;
        var user = await _signInManager.UserManager.FindByEmailAsync(email);
        return user;
    }
    
    
    [NonAction]
    public string GetClaimValueFromContext(string claimType)
    {
        var claims = (IEnumerable<Claim>)HttpContext.Items["Claims"];
        if (claims == null) return null;
        var claimValue = claims.Where(s => s.Type == claimType).FirstOrDefault().Value;
        return claimValue;
    }
}