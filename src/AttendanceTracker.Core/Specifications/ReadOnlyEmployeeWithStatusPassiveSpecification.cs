using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyEmployeeWithStatusPassiveSpecification : Specification<Employee>
    {
		public ReadOnlyEmployeeWithStatusPassiveSpecification()
		{
			Query
				.Include(j => j.JobPosition)
				.Where(e => e.Status == false);	
		}
	}
}

