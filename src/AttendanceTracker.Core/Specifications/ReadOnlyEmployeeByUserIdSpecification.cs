using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Entities.Account;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyEmployeeByUserIdSpecification : Specification<Employee>
    {
        public ReadOnlyEmployeeByUserIdSpecification(string userId)
        {
            Query
                .Where(s => s.UserId == userId).AsNoTracking();
        }
    }
}

