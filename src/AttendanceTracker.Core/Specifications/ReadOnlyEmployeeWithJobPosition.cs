using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyEmployeeWithJobPosition : Specification<Employee>
    {
		public ReadOnlyEmployeeWithJobPosition()
		{
			Query
				.Include(e => e.JobPosition).AsNoTracking();
		}
	}
}

