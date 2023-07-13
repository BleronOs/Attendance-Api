using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Entities.Account;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyChecksByManagerIdSpecification : Specification<Check>
    {
        public ReadOnlyChecksByManagerIdSpecification(int employeeId)
        {
            Query
                .Include(u => u.User)
                .Include(s => s.Card)
                .ThenInclude(e => e.Employee)
                .ThenInclude(em=>em.EmployeeManagment)
                .ThenInclude(m=>m.Manager)
                .Where(s=>s.Card.Employee.EmployeeManagment.Manager.EmployeeId == employeeId).AsNoTracking();
        }
    }
}