using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Entities.Account;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyEmployeeBasedInManagerId : Specification<Employee>
    {
        public ReadOnlyEmployeeBasedInManagerId(int employeeId)
        {
            Query
                .Include(j => j.JobPosition)
                .Include(m => m.EmployeeManagment).ThenInclude(m => m.Manager)
                .Where(s => s.EmployeeManagment.Manager.EmployeeId == employeeId && s.Status == true).AsNoTracking();
        }
    }
}

