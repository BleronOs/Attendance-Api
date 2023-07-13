using AttendanceTracker.Api.Interfaces;
using AttendanceTracker.Api.Models;
using AttendanceTracker.Api.ViewModels;
using AttendanceTracker.Core.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.ResultModels;
using System.Web;
using AttendanceTracker.Api.Configuration;
using AttendanceTracker.Infrastructure.Configuration.Email;
using Microsoft.AspNetCore.Authorization;

namespace AttendanceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ExtendedBaseController
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly PasswordReset _passwordReset;

    private readonly IEmployeeService _employeeService;
    private readonly IModulesAccessService _modulesAccessService;
    private readonly ITokenService _tokenService;
    private readonly IEmailSender _emailSender;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<Role> roleManager, PasswordReset passwordReset,
    IModulesAccessService modulesAccessService, IEmployeeService employeeService, ITokenService tokenService, IEmailSender emailSender) : base(signInManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
        _roleManager = roleManager;
        _modulesAccessService = modulesAccessService;
        _employeeService = employeeService;
        _passwordReset = passwordReset;
        _emailSender = emailSender;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SignIn login, CancellationToken cancellationToken)
    {
        var loginResult =
            await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

        if (!loginResult.Succeeded) return Unauthorized();
        var user = await _userManager.FindByNameAsync(login.Email);
        if (user.IsActive == false) return StatusCode(429);
        if (user.EmailConfirmed == false) return StatusCode(430);
        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user.UserName, roles[0]);
        var roleModules = new List<RoleModule>();
        var employeeUserId = new List<ResultUser>();
        var employee = await _employeeService.GetEmployeeByUserIdAsync(user.Id);
        employeeUserId.Add(new ResultUser
        {
            Employees = employee
        });

        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var module = await _modulesAccessService.GetModulesAccessForRoleAsync(role.Id);
            roleModules.Add(new RoleModule
            {
                RoleId = role.Id,
                Modules = module
            });
        }
        
        return Ok(new SignInSucceededViewModel { User = user, Token = token, RoleModules = roleModules, UserId = employeeUserId });
    }
    [Authorize]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Register register)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = register.Username,
            Email = register.Email,
            FirstName = register.FirstName,
            LastName = register.LastName,
            ModifyDate = DateTime.Now,
            RegistrationDate = DateTime.Now,
            IsActive = true,
            EmailConfirmed = true,
        };


        var result = await _userManager.CreateAsync(user, register.Password);
        if (!result.Succeeded) return NotFound(result.Errors);

        if (!string.IsNullOrEmpty(register.Role)) await _userManager.AddToRoleAsync(user, register.Role);

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user.UserName, roles[0]);
        return Ok(new SignInSucceededViewModel { User = user, Token = token });
    }

    [HttpPut("change/password")]
    public async Task<IActionResult> ChangePassword(ChangePassword model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByIdAsync(model.UserId);

        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (result.Succeeded)
        {
            return Ok();
        }
        else
        {
            return BadRequest(result.Errors.FirstOrDefault().Description);
        }
    }


    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmail confirmEmail)
    {
        var token = HttpUtility.UrlDecode(confirmEmail.Token);
        var email = HttpUtility.UrlDecode(confirmEmail.Email);
        if(string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
        {
            return BadRequest();
        }
        //var decodedToken = confirmEmail.Token.Replace(" ", "+");
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK);
            }
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Erroe", Message = "Ky user Nuk Egziston" });
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(PasswordResetModal model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return StatusCode(712);
        }
        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (!result.Succeeded)
        {
            return StatusCode(713);
        }
        return Ok("Passowrdi u ndryshua me sukses.");
    }
    [HttpPost("send-reset-password-url")]
    public async Task<IActionResult>SendPasswordResetEmail(GetEmailToResetPassword modal)
    {
        var user = await _userManager.FindByEmailAsync(modal.Email);
        if (user == null)
        {
            //Nese useri nuk gjendet, mos e dergo Linkun
            return StatusCode(799);
        }
        if(!(await _userManager.IsEmailConfirmedAsync(user)))
        {
            return StatusCode(798);
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        string callbackUrl = $"{_passwordReset.Url}?email={HttpUtility.UrlEncode(user.Email)}&token={HttpUtility.UrlEncode(token)}";
        string Message = $"Ju lutem klikoni ne linkun e me poshtem per te ndryshuar fjaleKalimin<br> <br/>  <a style='background:#3630a3;color:white;padding: 15px 25px 15px 25px;text-align:center; border-radius:15px;' href='{callbackUrl}'> Kliko per te Vendosur FjaleKalimin e Ri</a>";
         _emailSender.Send(user.Email, "Vendose Fjalkalimin e Ri", Message);
        return Ok();
    }
    [HttpGet("roles/list")]
    public IActionResult GetRoles()
    {
        var roles = _roleManager.Roles.ToList();
        return Ok(roles);
    }
}