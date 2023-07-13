using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;
namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyManagersWithoutEmployeesSpecifications : Specification<Employee>
    {
		public ReadOnlyManagersWithoutEmployeesSpecifications()
		{
			Query
				.Where(s => s.EmployeeManagment == null && s.Status == true)
				.Where(m=>m.Manager==null);
        }
	}
}

