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
    public class ModulesAccessController : ExtendedBaseController
    {
        private readonly IModulesAccessService _modulesAccessService;
        public ModulesAccessController(SignInManager<User> signInManager,
            IModulesAccessService modulesAccessService) : base(signInManager)
        {
            _modulesAccessService = modulesAccessService;
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetModulesAccess(CancellationToken cancellationToken)
        {
            var modules = await _modulesAccessService.GetModulesAccessesAsync(cancellationToken);
            return Ok(modules);
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddModulesAccess([FromBody] AddModulesAccess addModulesAccess, CancellationToken cancellationToken = default)
        {
            var create = await _modulesAccessService.AddModuleAccessAsync(addModulesAccess.RoleId, addModulesAccess.ModuleId);
            if (create) return Ok();
            return BadRequest();
        }
        [HttpPut("update/module/access/{roleid}/{moduleId}")]
        public async Task<IActionResult> UpdateStatusCardTrue(string roleid, int moduleId, CancellationToken cancellationToken)
        {
            await _modulesAccessService.UpdateInsertModuleAccessAsync(roleid, moduleId, cancellationToken);
            return Ok();
        }
        [HttpGet("list/role/modules/{roleId}")]
        public async Task<IActionResult> GetModulesAccessRoleSpecification(string roleId,CancellationToken cancellationToken)
        {
            var modules = await _modulesAccessService.GetModulesAccessForRoleAsync(roleId, cancellationToken);
            return Ok(modules);
        }
      
    }
}

