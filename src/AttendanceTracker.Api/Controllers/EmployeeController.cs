using System;
using AttendanceTracker.Api.Interfaces;
using AttendanceTracker.Api.Models;
using AttendanceTracker.Api.ViewModels;
using AttendanceTracker.Core.Entities.Account;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using AttendanceTracker.Infrastructure.Configuration.Email;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using AttendanceTracker.Api.Configuration;
using System.Web;
using AttendanceTracker.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace AttendanceTracker.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ExtendedBaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<User> _userManager;
        private readonly ICardService _cardService;
        
        private readonly ITokenService _tokenService;
        private readonly IEmailSender _emailSender;
        private readonly EmailConfirmation _emailConfirmation;


        public EmployeeController(SignInManager<User> signInManager, IEmployeeService employeeService, UserManager<User> userManager,
        ITokenService tokenService, IEmailSender emailSender, EmailConfirmation emailConfirmation) : base(signInManager)
        {
            _employeeService = employeeService;
            _userManager = userManager;
            _tokenService = tokenService;
            _emailSender = emailSender;
            _emailConfirmation = emailConfirmation;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetEmployee(CancellationToken cancellationToken = default)
        {
            var employees = await _employeeService.GetEmployeesAsync(cancellationToken);
            return Ok(employees);
        }
        [HttpGet("deactivated-and-with-no-card")]
        public async Task<IActionResult> GetEmployeesWithoutCardAndStatusPasive(CancellationToken cancellationToken = default)
        {
            var employees = await _employeeService.GetEmployeesWithoutCardAndStatusPasiveAsync(cancellationToken);
            return Ok(employees);
        }

        [HttpGet("employee-job-position")]
        public async Task<IActionResult> GetEmployeeJobPosition(CancellationToken cancellationToken = default)
        {
            var employeejobposition = await _employeeService.GetEmployeeJobPositionAsync(cancellationToken);
            return Ok(employeejobposition);
        }

        [HttpGet("employee-with-status-active")]
        public async Task<IActionResult> GetEmployeeWithStatusActive(CancellationToken cancellationToken = default)
        {
            var employeeWithStatusActive = await _employeeService.GetEmployeesWithStatusActiveAsync(cancellationToken);
            return Ok(employeeWithStatusActive);
        }

        [HttpGet("employee-with-status-passive")]
        public async Task<IActionResult> GetEmployeeWithStatusPassive(CancellationToken cancellationToken = default)
        {
            var employeeWithStatusPassive = await _employeeService.GetEmployeesWithStatusPassiveAsync(cancellationToken);
            return Ok(employeeWithStatusPassive);
        }
        [NonAction]
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var res = "";
            Random rnd = new Random();
            while (0 < length--)
            {
                res += (valid[rnd.Next(valid.Length)]);
            }
            return res.ToString().ToLower();
        }

        //[NonAction]
        //public IActionResult SendEmail(string employeeEmail, string password,string? message)
        //{
        //    var body = $"Username eshte: {employeeEmail} dhe Password eshte: {password}, Per ta Konfirmuar Emailen tuaj ju lutem klikoni ne linkun me poshte: {message}";
        //    var email = new MimeMessage();
        //    email.From.Add(MailboxAddress.Parse("attendancetrackerui@gmail.com"));
        //    email.To.Add(MailboxAddress.Parse(employeeEmail));
        //    email.Subject = "Te Dhenat Per Qasje Ne Applikacion";
        //    email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
            

        //    using var smtp = new SmtpClient();

        //    smtp.Connect("smtp.gmail.com.", 587, SecureSocketOptions.StartTls);
        //    smtp.Authenticate("attendancetrackerui@gmail.com", "byxrrnmenzvyaozx");
        //    smtp.Send(email);
        //    smtp.Disconnect(true);

        //    return Ok();

        //}

        [HttpPost("add")]
        public async Task<IActionResult> AddEmployees([FromBody] AddEmployee addEmployee, CancellationToken cancellationToken = default)
        {
            string userId = Guid.NewGuid().ToString();
            var result = await Register(addEmployee.FirstName, addEmployee.LastName, addEmployee.Email, addEmployee.Role, userId, cancellationToken);
            if (result == null) return NotFound();

            var create = await _employeeService.AddEmployeesAsync
                (
                    addEmployee.FirstName,
                    addEmployee.LastName,
                    addEmployee.BirthDate,
                    addEmployee.PersonalNumber,
                    addEmployee.Address,
                    addEmployee.Email,
                    addEmployee.PhoneNumber,
                    addEmployee.PositionId,
                    addEmployee.Status,
                    userId
                );
            if (create)
            {

                return Ok();
            }
            return BadRequest();

        }

        private async Task<IActionResult> Register(string firstname, string lastname, string email, string roleName, string userId = null, CancellationToken cancellationToken = default)
        {
            string password = CreatePassword(8);
            var user = new User
            {
                //Id = Guid.NewGuid().ToString(),
                Id = userId ?? Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                FirstName = firstname,
                LastName = lastname,
                ModifyDate = DateTime.Now,
                RegistrationDate = DateTime.Now,
                IsActive = true,
                EmailConfirmed = false,
            };
        

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) return NotFound(result.Errors);

            if (!string.IsNullOrEmpty(roleName)) await _userManager.AddToRoleAsync(user, roleName);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user.UserName, roles[0]);

            var tokens = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenForNewPassword = await _userManager.GeneratePasswordResetTokenAsync(user);
            //var confirmationLink = Url.Action("confirm-email", "Account", new { token = tokens, email = user.Email }, Request.Scheme);
            var confirmationLink = $"{_emailConfirmation.Url}?confirmEmailToken={HttpUtility.UrlEncode(tokens)}&confirmEmail={HttpUtility.UrlEncode(email)}&resetPassword={HttpUtility.UrlEncode(tokenForNewPassword)}";
            string message = $"<br> <br/> <a style='background:#3630a3;color:white;padding: 15px 25px 15px 25px;text-align:center; border-radius:15px;' href='{confirmationLink}'> Kliko per te konfirumar emailen</a>";

            var emailBody = $"Username eshte: {email} dhe Password eshte: {password}, Per ta Konfirmuar Emailen tuaj ju lutem klikoni ne butonin me poshte: {message}";

            _emailSender.Send(user.Email, "Te Dhenat Per Qasje Ne Applikacion", emailBody);
            return Ok(new SignInSucceededViewModel { User = user, Token = token });
        }

        [HttpPut("update/status/{id}")]
        public async Task<IActionResult> UpdateEmployeeStatus(int id, CancellationToken cancellationToken)
        {
            var hasActiveManagerResult = await _employeeService.UpdateEmployeeStatusToFalseAsync(id, cancellationToken);
            if (hasActiveManagerResult.HasActiveManager)
            {
                return BadRequest();
            }

            // delete the user from the register
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return Ok();
        }
        
        
       /*[HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeViewModel model, int id, CancellationToken cancellationToken = default)
        {
            await _employeeService.UpdateEmployeeAsync
                (

                    id,
                    model.FirstName,
                    model.LastName,
                    model.BirthDate,
                    model.PersonalNumber,
                    model.Address,
                    model.Email,
                    model.PhoneNumber,
                    model.PositionId,
                    model.Status
                );
            return Ok();
        }*/
       
       [HttpPut("update/{id}")]
       public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeViewModel model, int id, CancellationToken cancellationToken = default)
       {
           var user = await _userManager.FindByNameAsync(model.Email);
           if (user == null)
           {
               return NotFound();
           }
           user.FirstName = model.FirstName;
           user.LastName = model.LastName;
            if(model.NewEmail != user.Email)
            {
                user.EmailConfirmed = false;
                user.Email = model.NewEmail;
                user.UserName = model.NewEmail;
                SendConfirmEmail(user,model.NewEmail);
            }
           await _userManager.UpdateAsync(user);
           await _employeeService.UpdateEmployeeAsync
           (
               id,
               model.FirstName,
               model.LastName,
               model.BirthDate,
               model.PersonalNumber,
               model.Address,
               model.NewEmail,
               model.PhoneNumber,
               model.PositionId,
               model.Status
           );
           return Ok();
       }
     
        [HttpPut("update-employee-to-send-to-archive/{id}/{note}/{email}")]
        public async Task<IActionResult> UpdateEmployeeToSendToArchive(int id, string note, string email,CancellationToken cancellationToken = default)
        {
            var hasActiveManagerResult = await _employeeService.UpdateEmployeeStatusToFalseAsync(id, cancellationToken);
            if (hasActiveManagerResult.HasActiveManager)
            {
                return BadRequest();
            }

            await _employeeService.UpdateEmployeeToSendToArchiveAsync
                (
                    id,
                    note,
                    cancellationToken
                );

            var user = await _userManager.FindByEmailAsync(email);

            // If the user exists, update status the user
            if (user != null)
            {
                user.IsActive = false;
                user.Email = "old" + email;
                user.UserName = "old" + email;
                user.NormalizedEmail ="old" + email;
                user.NormalizedUserName = "old" + email;
                user.EmailConfirmed = false;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _cardService.UpdateCardWhenEmployeeIsPassiveAsync(id, cancellationToken);
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return Ok();
        }

        [HttpGet("not-linked-with-manager")]
        public async Task<IActionResult> GetEmployeeWithoutManager(CancellationToken cancellationToken = default)
        {
            var employeeManagment = await _employeeService.GetEmployeeWithoutManagerAsync(cancellationToken);
            return Ok(employeeManagment);
        }

        [HttpGet("not-linked-manager-twice")]
        public async Task<IActionResult> GetManagerNotTwice(CancellationToken cancellationToken = default)
        {
            var employeeManagment = await _employeeService.GetManagerNotTwiceAsync(cancellationToken);
            return Ok(employeeManagment);
        }

        [HttpGet("employee-not-two-rolesjobs")]
        public async Task<IActionResult> GetEmployeeWithoutTwoRolesJobs(CancellationToken cancellationToken = default)
        {
            var employeesWithoutTwoJobs = await _employeeService.GetEmployeeNotAsManagerAsync(cancellationToken);
            return Ok(employeesWithoutTwoJobs);
        }

        [HttpPut("employee-managers/{id}")]
        public async Task<IActionResult> GetManagersThatAreNotActiveEmployee(int id, CancellationToken cancellationToken = default)
        {
            var managersThatAreNotActiveEmployee = await _employeeService.UpdateManagerWhileEmployeeIsActiveAsync(id,cancellationToken);
            if(managersThatAreNotActiveEmployee == false)
            {
                return BadRequest();
            }

            return Ok(managersThatAreNotActiveEmployee);
        }

        [HttpGet("employee-that-are-not-manager")]
        public async Task<IActionResult> GetEmployeeThatAreNotManager(CancellationToken cancellationToken = default)
        {
            var employeesThatAreNotManager = await _employeeService.GetEmployeeThatAreNotManagerAsync(cancellationToken);
            return Ok(employeesThatAreNotManager);
        }

        [HttpGet("employee-not-linked-into-employee-managment")]
        public async Task<IActionResult> GetEmployeeNotLinkedIntoEmployeeManagment(CancellationToken cancellationToken = default)
        {
            var employeesNotLinkedIntoEmployeeManagment = await _employeeService.GetEmployeeNotLinkedIntoEmployeeManagmentAsync(cancellationToken);
            return Ok(employeesNotLinkedIntoEmployeeManagment);
        }
        [HttpGet("employee-by-manager-id/{employeeId}")]
        public async Task<IActionResult> GetEmployeeBasedInManagerID(int employeeId, CancellationToken cancellationToken = default)
        {
            var managerByRoleId = await _employeeService.GetEmployeeBasedInManagerIdAsync(employeeId, cancellationToken);
            return Ok(managerByRoleId);
        }
        private async void SendConfirmEmail(User user, string email)
        {
            var tokens = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"{_emailConfirmation.Url}?confirmEmailToken={HttpUtility.UrlEncode(tokens)}&confirmEmail={HttpUtility.UrlEncode(email)}&resetPassword=skip";
            string message = $"<br> <br/> <a style='background:#3630a3;color:white;padding: 15px 25px 15px 25px;text-align:center; border-radius:15px;' href='{confirmationLink}'> Kliko per te konfirumar emailen</a>";

            var emailBody = $"Username i ri eshte: {email}, Per ta Konfirmuar Emailen tuaj ju lutem klikoni ne butonin me poshte: {message}";

           await _emailSender.SendAsync(user.Email, "Konfirmimi i Email-es", emailBody);

        }

    }
}

