using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadonlyAllPasiveEmployeesByManagerSpecification : Specification<Employee>
    {
        public ReadonlyAllPasiveEmployeesByManagerSpecification(int managerId)
        {

            Query
                .Include(m => m.EmployeeManagment).ThenInclude(m => m.Manager)
                .Where(s => s.EmployeeManagment.Manager.EmployeeId == managerId && s.Status == false).AsNoTracking();
        }
    }
}

