using System;
using AttendanceTracker.Core.Entities;
using Ardalis.Specification;
namespace AttendanceTracker.Core.Specifications
{
	public class GetEmployeeToChecks: Specification<Check>
	{
		public GetEmployeeToChecks()
		{
			Query
				.Include(u => u.User)
				.Include(s => s.Card)
				.ThenInclude(e => e.Employee).AsNoTracking();
		}
	}
}

