using System;
using AttendanceTracker.Core.Entities;
using Ardalis.Specification;
namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyEmployeeCheckInOutByEmployeeIdSpecification : Specification<Check>
    {
        public ReadOnlyEmployeeCheckInOutByEmployeeIdSpecification(DateTime dateTime, int employeeId)
        {
            var currentDateTime = dateTime;
            var firstTimeOfDay = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0, 0);
            var endOfTheDay = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 23, 59, 59);
            Query
                .Where(s => s.CheckDateTime >= firstTimeOfDay && s.CheckDateTime <= endOfTheDay)
                .Include(s => s.Card)
                .ThenInclude(e => e.Employee)
                .Where(s => s.Card.Employee.Id == employeeId).AsNoTracking();
        }
    }
}

