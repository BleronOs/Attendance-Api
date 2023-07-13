using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyEmployeeNotAsManager : Specification<Employee>
    {
		public ReadOnlyEmployeeNotAsManager()
		{
			Query
				.Include(e => e.Manager)
				.Include(s => s.EmployeeManagment)
				.Where(em => em.EmployeeManagment.Employee == null);
        }
	}
}

