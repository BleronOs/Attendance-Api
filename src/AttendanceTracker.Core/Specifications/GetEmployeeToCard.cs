using System;
using AttendanceTracker.Core.Entities;
using Ardalis.Specification;
namespace AttendanceTracker.Core.Specifications
{
	public class GetEmployeeToCard: Specification<Card>
	{
		public GetEmployeeToCard()
		{
			Query.Include(e => e.Employee)
				.Where(e=>e.Status==true)
				.Where(e=>e.Employee.Status==true)
				.AsNoTracking();
		}
	}
}

