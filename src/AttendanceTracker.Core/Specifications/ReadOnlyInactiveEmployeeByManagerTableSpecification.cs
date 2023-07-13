using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyInactiveEmployeeByManagerTableSpecification : Specification<Manager>
    {
		public ReadOnlyInactiveEmployeeByManagerTableSpecification(int employeeId)
		{
            Query.Include(s => s.Employee)
               .Where(s => s.Id == employeeId && s.Employee.Status == true)
               .AsNoTracking();
        }
	}
}

