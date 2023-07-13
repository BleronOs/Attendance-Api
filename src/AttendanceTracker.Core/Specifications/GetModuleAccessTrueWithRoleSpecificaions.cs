using System;
using AttendanceTracker.Core.Entities;
using Ardalis.Specification;
namespace AttendanceTracker.Core.Specifications
{
	public class GetModuleAccessTrueWithRoleSpecificaions:Specification<ModulesAccess>
	{
		public GetModuleAccessTrueWithRoleSpecificaions(string role)
		{
            Query
				.Where(s => s.RoleId == role && s.HasAccess == true)
				.AsNoTracking();
        }
	}
}

