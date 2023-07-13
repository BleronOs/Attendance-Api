using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyInactiveJobPositionsByEmployee : Specification<Employee>
    {
        public ReadOnlyInactiveJobPositionsByEmployee(int id)
        {
            Query.Include(s => s.JobPosition)
               .Where(s => s.JobPosition != null && s.JobPosition.Id == id && s.Status == true)
               .AsNoTracking();
        }
    }
}

