using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadonlyAllEmployeesByManagerSpecification : Specification<Employee>
    {
        public ReadonlyAllEmployeesByManagerSpecification(int managerId)
        {
            Query
                .Include(m => m.EmployeeManagment).ThenInclude(m => m.Manager)
                .Where(s => s.EmployeeManagment.Manager.EmployeeId == managerId).AsNoTracking();
        }
    }
}

