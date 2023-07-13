using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyIfExistAnEmployeeInManagmentSpecification : Specification<EmployeeManagment>
    {
        public ReadOnlyIfExistAnEmployeeInManagmentSpecification(int id)
        {
            Query.Where(e => e.ManagerId == id).AsNoTracking();
        }
    }
}
