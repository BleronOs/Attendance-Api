using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyManagersSpecifications : Specification<Employee>
    {
		public ReadOnlyManagersSpecifications()
		{
            Query
                .Include(m => m.Manager)
                .Where(ma => ma.Manager == null);
        }
	}
}

