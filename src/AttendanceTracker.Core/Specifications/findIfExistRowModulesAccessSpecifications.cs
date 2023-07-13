using System;
using AttendanceTracker.Core.Entities;
using Ardalis.Specification;
namespace AttendanceTracker.Core.Specifications
{
	public class findIfExistRowModulesAccessSpecifications:Specification<ModulesAccess>
	{
		public findIfExistRowModulesAccessSpecifications(string roleId, int moduleId)
		{
			Query.Where(s => s.ModuleId == moduleId && s.RoleId == roleId)
			     .AsNoTracking();
		}
	}
}

