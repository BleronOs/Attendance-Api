using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyManagersWithStatusPassiveSpecification : Specification<Manager>
    {
		public ReadOnlyManagersWithStatusPassiveSpecification()
		{
			Query
				.Include(e => e.Employee)
				.Where(m => m.Status == false);
		}
	}
}

