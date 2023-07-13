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
    public class ManagerController : ExtendedBaseController
    {
        private readonly IManagerService _managerService;

        public ManagerController(SignInManager<User> signInManager, IManagerService managerService) : base(signInManager)
        {
            _managerService = managerService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetManagers(CancellationToken cancellationToken)
        {
            var managers = await _managerService.GetManagersAsync(cancellationToken);
            return Ok(managers);
        }
        [HttpGet("employee-list")]
        public async Task<IActionResult> GetEmployee(CancellationToken cancellationToken = default)
        {
            var employees = await _managerService.GetEmployeeManagmentAsync(cancellationToken);
            return Ok(employees);
        }

        [HttpGet("manager-with-status-active")]
        public async Task<IActionResult> GetManagersWithStatusActive(CancellationToken cancellationToken = default)
        {
            var managersWithStatusActive = await _managerService.GetManagersWithStatusActiveAsync(cancellationToken);
            return Ok(managersWithStatusActive);
        }

        [HttpGet("manager-with-status-passive")]
        public async Task<IActionResult> GetManagersWithStatusPassive(CancellationToken cancellationToken = default)
        {
            var managersWithStatusPassive = await _managerService.GetManagersWithStatusPassiveAsync(cancellationToken);
            return Ok(managersWithStatusPassive);
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddManagers([FromBody] AddManager addManager ,CancellationToken cancellationToken = default)
        {
            var create = await _managerService.AddManagersAsync(addManager.EmployeeId, addManager.Status);
            if(create) return Ok();
            return BadRequest();
        }

        //[HttpDelete("delete/{id}")]
        //public async Task<IActionResult> DeleteManagers(int id, CancellationToken cancellationToken = default)
        //{
        //    await _managerService.DeleteManagersAsync(id);
        //    return Ok();
        //

        [HttpPut("update/status/{id}")]
        public async Task<IActionResult> UpdateManagersStatusToFalse(int id, CancellationToken cancellationToken)
        {
            var result = await _managerService.UpdateManagerStatusToFalseAsync(id, cancellationToken);
            if (!result)
            {
                return StatusCode(900);
            }
            return Ok();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateManagers([FromBody] ManagersViewModels model, int id, CancellationToken cancellationToken = default)
        {
            await _managerService.UpdateManagersAsync(id, model.EmployeeId, model.Status);
            return Ok();
        }
	}
}

