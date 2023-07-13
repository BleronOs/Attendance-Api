using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
    public class ActivateCardIfEmployeeIsActiveSpecification : Specification<Card>
    {
        public ActivateCardIfEmployeeIsActiveSpecification(int employeeId)
        {
            Query.Include(s => s.Employee)
               .Where(s => s.Employee.Id == employeeId && s.Employee.Status == true)
               .AsNoTracking();
        }
    }
}

