using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Entities.Account;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyCardStatusFalseByManagerIdSpecification : Specification<Card>
    {
        public ReadOnlyCardStatusFalseByManagerIdSpecification(int employeeId)
        {
            Query

                .Include(e => e.Employee)
                .ThenInclude(m => m.EmployeeManagment).ThenInclude(m => m.Manager).ThenInclude(e => e.Employee)
                .Where(s => s.Employee.EmployeeManagment.Manager.Employee.Id == employeeId)
                .Where(s => s.Status == false)
                .Where(s => s.Employee.Status == true)
                .AsNoTracking();
        }
    }
}
