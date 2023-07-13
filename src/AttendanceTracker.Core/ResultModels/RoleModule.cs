using System;
using AttendanceTracker.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace AttendanceTracker.Core.ResultModels
{
	public class RoleModule
	{
		public string RoleId { get; set; }
        public IReadOnlyList<ModulesAccess> Modules { get; set; }
    }
}

