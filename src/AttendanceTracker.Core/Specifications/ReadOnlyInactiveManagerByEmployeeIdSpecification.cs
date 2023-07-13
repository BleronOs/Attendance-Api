using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public sealed class ReadonlyInactiveManagerByEmployeeIdSpecification : Specification<Employee>
    {
		public ReadonlyInactiveManagerByEmployeeIdSpecification(int managerId)
		{
            Query.Include(s => s.Manager)
                .Where(s => s.Id == managerId && s.Manager.Status == true)
                .AsNoTracking();
        }
	}
}

