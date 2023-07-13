using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyEmployeeThatAreNotManagers : Specification<Employee>
    {
		public ReadOnlyEmployeeThatAreNotManagers()
		{
			Query
				.Include(m => m.Manager)
				.Where(ma => ma.Manager == null);
		}
	}
}

