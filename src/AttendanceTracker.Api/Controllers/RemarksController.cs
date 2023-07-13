using System;
using AttendanceTracker.Api.Models;
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
    public class RemarksController : ExtendedBaseController
	{
		private readonly IRemarksService _remarksService;
		public RemarksController(SignInManager<User> signInManager, IRemarksService remarksService): base(signInManager)
        {
			_remarksService = remarksService;
		}
        [HttpGet("list/{id}")]
        public async Task<IActionResult> GetRemarks(int id, CancellationToken cancellationToken)
        {
            var modules = await _remarksService.GetRemarksAsync(id);
            return Ok(modules);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddCards([FromBody] AddRemarks model, CancellationToken cancellationToken)
        {
            var create = await _remarksService.AddRemarksAsync(model.EmployeeId,model.Notes, cancellationToken);
            if (create) return Ok();
            return BadRequest();
        }

    }
}

