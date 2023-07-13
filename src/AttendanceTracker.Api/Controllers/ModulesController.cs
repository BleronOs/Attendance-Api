using System;
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
    public class ModulesController : ExtendedBaseController
    {
        private readonly IModulesService _modulesService;

        public ModulesController(SignInManager<User> signInManager, IModulesService modulesService) : base(signInManager)
        {
            _modulesService = modulesService;
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetModules(CancellationToken cancellationToken)
        {
            var modules = await _modulesService.GetModulesAsync(cancellationToken);
            return Ok(modules);
        }
    }
}

