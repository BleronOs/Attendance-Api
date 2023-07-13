
using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
    public sealed class ReadOnlyInactiveManagersWhileEmployeeIsInactiveSpecifications : Specification<Employee>
    {
        public ReadOnlyInactiveManagersWhileEmployeeIsInactiveSpecifications(int employeeId)
        {
            Query.Include(s => s.Manager)
                .Where(s => s.Manager.EmployeeId == employeeId && s.Status == true)
                .AsNoTracking();
        }
    }
}

