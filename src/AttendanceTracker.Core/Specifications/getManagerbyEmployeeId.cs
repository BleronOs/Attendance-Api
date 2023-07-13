using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
    public sealed class getManagerbyEmployeeId : Specification<Manager>
    {
        public getManagerbyEmployeeId(int employeeId)
        {
            Query.Where(e => e.EmployeeId == employeeId);
        }
    }
}


