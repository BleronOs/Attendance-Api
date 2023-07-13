
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using AttendanceTracker.Core.Entities.Account;
using AttendanceTracker.Api.Models;
using AttendanceTracker.Core.Interfaces;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace AttendanceTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController : ExtendedBaseController
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IModulesAccessService _modulesAccessService;

        public RoleController(SignInManager<User> signInManager, RoleManager<Role> roleManager,
            IModulesAccessService modulesAccessService) : base(signInManager)
        {
            _roleManager = roleManager;
            _modulesAccessService = modulesAccessService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateRole([FromBody] AddRole addRole, CancellationToken cancellationToken)
        {
            string roleId = Guid.NewGuid().ToString();
            var roleExists = await _roleManager.RoleExistsAsync(addRole.Name);
            if (roleExists)
            {
                return StatusCode(422);
            }

            var result = await _roleManager.CreateAsync(new Role()
            {
                Id = roleId,
                Name = addRole.Name,
                NormalizedName = addRole.Name.ToUpper()
            });

            if (result.Succeeded)
            {
                await AddModuleAccessTrue(roleId, 13, cancellationToken);
                return Ok();

            }
            else
            {
                return StatusCode(421);
            }
        }
        [NonAction]
        public async Task<IActionResult> AddModuleAccessTrue(string roleid, int moduleId, CancellationToken cancellationToken)
        {
            await _modulesAccessService.AddModuleAccessAsync(roleid, moduleId, cancellationToken);
            return Ok();
        }

    }

}