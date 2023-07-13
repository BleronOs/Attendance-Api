using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Entities.Account;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyTheDataInTheFrameworkOfTheRoleSpecification : Specification<EmployeeManagment>
    {
        public ReadOnlyTheDataInTheFrameworkOfTheRoleSpecification(int employeeId)
        {
            Query
                .Include(m => m.Manager)
                .ThenInclude(s => s.Employee)
                .Include(e=>e.Employee)
                .Where(s => s.Manager.EmployeeId == employeeId)
                .Where(s=>s.Employee.Status==true)
                .AsNoTracking();
        }
    }
}

