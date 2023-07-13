using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyActivateCardByEmployeeId : Specification<Card>
    {
        public ReadOnlyActivateCardByEmployeeId(int employeeId)
        {
            Query
               .Where(s => s.EmployeeId == employeeId && s.Status == true)
               .AsNoTracking();
        }
    }
}


