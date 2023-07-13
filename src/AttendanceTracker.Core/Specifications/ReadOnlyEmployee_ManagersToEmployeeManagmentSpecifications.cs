using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;
namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyEmployee_ManagersToEmployeeManagmentSpecifications : Specification<EmployeeManagment>
    {
		public ReadOnlyEmployee_ManagersToEmployeeManagmentSpecifications()
		{
			Query
				.Include(e => e.Manager).ThenInclude(m => m.Employee)
				.Include(s => s.Employee)
				.Where(em => em.Employee.Status == true && em.Manager.Status == true).AsNoTracking();
		}
	}
}

