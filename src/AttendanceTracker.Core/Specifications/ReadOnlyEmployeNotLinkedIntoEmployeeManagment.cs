using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyEmployeNotLinkedIntoEmployeeManagment : Specification<Employee>
    {
		public ReadOnlyEmployeNotLinkedIntoEmployeeManagment()
		{


			Query
				.Include(em => em.EmployeeManagment)
				.Include(m => m.Manager)
				.Where(e => e.Status == true || e.Manager.Status == true);
		}
	}
}

