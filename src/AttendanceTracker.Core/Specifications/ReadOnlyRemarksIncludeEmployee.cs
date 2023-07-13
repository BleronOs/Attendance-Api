using System;
using AttendanceTracker.Core.Entities;
using Ardalis.Specification;
namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyRemarksIncludeEmployee : Specification<Remarks>
    {
        public ReadOnlyRemarksIncludeEmployee(int id)
        {
            Query
                .Include(e => e.Employee)
                .Where(e => e.Employee.Id == id)
                .AsNoTracking();
        }
    }
}


