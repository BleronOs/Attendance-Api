using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;
namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyEmloyeeManagmentByManagersSpecifications : Specification<Manager>
    {
		public ReadOnlyEmloyeeManagmentByManagersSpecifications()
		{
			Query
				.Include(e => e.Employee).AsNoTracking();
		}
	}
}

