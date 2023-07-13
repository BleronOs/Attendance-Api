using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyManagersWithStatusActiveSpecifications : Specification<Manager>
    {
		public ReadOnlyManagersWithStatusActiveSpecifications()
		{
			Query
				.Include(e=>e.Employee)
				.Where(m => m.Status == true);
		}
	}
}

