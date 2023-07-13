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
    public class JobPositionController : ExtendedBaseController
	{
        private readonly IJobPositionService _jobPositionService;
        public JobPositionController(SignInManager<User> signInManager, IJobPositionService jobPositionService)
            : base(signInManager)
        {
            _jobPositionService = jobPositionService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewJobPosition([FromBody] AddJobPosition addJobPosition,
            CancellationToken cancellationToken = default)
        {
            var created = await _jobPositionService.AddJobPositionAsync(addJobPosition.PositionName, cancellationToken);
            if (created) return Ok();
            return BadRequest();
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetJobPositions(CancellationToken cancellationToken)
        {
            var jobPositions = await _jobPositionService.GetJobPositionsAsync(cancellationToken);
            return Ok(jobPositions.OrderByDescending(x=>x.Id));
        }

        [HttpGet("status/true")]
        public async Task<IActionResult> GetJobPositionStatusTrue(CancellationToken cancellationToken)
        {
            var jobPositionTrue = await _jobPositionService.GetJobPositionsStatusTrueAsync(cancellationToken);
            return Ok(jobPositionTrue.OrderByDescending(x => x.Id));
        }

        [HttpPut("delete/{id}")]
        public async Task<IActionResult> UpdateJobPositionStatus(int id, CancellationToken cancellationToken)
        {
            var hasActiveManagerResult = await _jobPositionService.UpdateJobPositionStatusAsync(id, cancellationToken);
            if (hasActiveManagerResult.HasActiveJobPosition)
            {
                return StatusCode(900);
            }
            return Ok();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UdpateJobPosition([FromBody] JobPositionViewModel model, int id, CancellationToken cancellationToken ) { 
            await _jobPositionService.UpdateJobPositionAsync(id, model.PositionName,model.Status, cancellationToken);
            return Ok();
        }
    }
}

