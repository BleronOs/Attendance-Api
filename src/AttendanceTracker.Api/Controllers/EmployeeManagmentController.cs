using System;
using AttendanceTracker.Api.Interfaces;
using AttendanceTracker.Api.Models;
using AttendanceTracker.Api.ViewModels;
using AttendanceTracker.Core.Entities.Account;
using AttendanceTracker.Core.Interfaces;
using AttendanceTracker.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceTracker.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeManagmentController : ExtendedBaseController
	{
   
            private readonly IEmployeeManagmentService _employeeManagmentService;

            public EmployeeManagmentController(SignInManager<User> signInManager, IEmployeeManagmentService employeeManagmentService) : base(signInManager)
            {
                _employeeManagmentService = employeeManagmentService;
            }

            [HttpGet("list")]
            public async Task<IActionResult> GetEmployeeManagment(CancellationToken cancellationToken = default)
            {
                var employeeManagment = await _employeeManagmentService.GetEmployeeManagmentAsync(cancellationToken);
                return Ok(employeeManagment);
            }
         
        [HttpPost("add")]
            public async Task<IActionResult> AddEmployeeManagment([FromBody] AddEmployeeManagment addEmployeeManagment, CancellationToken cancellationToken = default)
            {
                var create = await _employeeManagmentService.AddEmployeeManagmentAsync
                    (
                        addEmployeeManagment.ManagerId,
                        addEmployeeManagment.EmployeeId
                    );
                if (create) return Ok();
                return BadRequest();

            }

            [HttpDelete("delete/{id}")]
            public async Task<IActionResult> DeleteEmployeeManagment(int id, CancellationToken cancellationToken = default)
            {
                await _employeeManagmentService.DeleteEmployeeManagmentAsync(id);
                return Ok();
            }

            [HttpPut("update/{id}")]
            public async Task<IActionResult> UpdateEmployeeManagment([FromBody] EmployeeManagmentViewModel model, int id, CancellationToken cancellationToken = default)
            {
                await _employeeManagmentService.UpdateEmployeeManagmentAsync
                    (

                        id,
                        model.ManagerId,
                        model.EmployeeId

                    );
                return Ok();
            }
        [HttpGet("manager-by-role-id/{employeeId}")]
        public async Task<IActionResult> GetManagerByRoleId(int employeeId, CancellationToken cancellationToken = default)
        {
            var managerByRoleId = await _employeeManagmentService.GetManagerByRoleIdAsync(employeeId, cancellationToken);
            return Ok(managerByRoleId);
        }
    }
    }
