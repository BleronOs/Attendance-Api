

using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;
using AttendanceTracker.Core.Entities.Account;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyCardsForTheFirstTimeInsertedSpecification : Specification<Card>
    {
        public ReadOnlyCardsForTheFirstTimeInsertedSpecification(int employeeId)
        {
            Query
                .Where(s => s.EmployeeId == employeeId && s.Status == true)
                .AsNoTracking();
            ;
        }
    }
}