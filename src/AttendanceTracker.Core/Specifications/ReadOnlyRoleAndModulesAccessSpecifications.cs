using System;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Entities.Account;
using Ardalis.Specification;
namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyRoleAndModulesAccessSpecifications:Specification<Role>
	{
		public ReadOnlyRoleAndModulesAccessSpecifications()
		{
			Query.Include(s => s.ModulesAccesses).AsNoTracking();
		}
	}
}

