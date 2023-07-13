using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyEmployeWithStatusActiveSpecifications : Specification<Employee>
    {
		public ReadOnlyEmployeWithStatusActiveSpecifications()
		{
			Query
				.Include(e => e.JobPosition)
				.Where(e => e.Status == true);
		}
	}
}

