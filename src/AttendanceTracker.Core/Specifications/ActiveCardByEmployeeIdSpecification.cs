using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public sealed class ActiveCardByEmployeeIdSpecification : Specification<Card>
	{
		public ActiveCardByEmployeeIdSpecification(int employeeId, bool isReadonly)
		{
			Query.Where(e => e.EmployeeId == employeeId && e.Status == true);

			if (isReadonly) Query.AsNoTracking();
		}
    }
}

