using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Entities.Account;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyEmployeeChecksByEmployeeIdSpecification : Specification<Check>
    {
        public ReadOnlyEmployeeChecksByEmployeeIdSpecification(int employeeId)
        {
            Query
                .Include(u => u.User)
                .Include(s => s.Card)
                .ThenInclude(e => e.Employee)
                .Where(s => s.Card.Employee.Id == employeeId).AsNoTracking();
        }
    }
}
