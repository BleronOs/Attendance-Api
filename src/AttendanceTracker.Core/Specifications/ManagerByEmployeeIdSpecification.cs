using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public class ManagerByEmployeeIdSpecification : Specification<Manager>
	{
		public ManagerByEmployeeIdSpecification(int employeeId)
		{
			Query.Where(e => e.EmployeeId == employeeId);
		}
	}
}

