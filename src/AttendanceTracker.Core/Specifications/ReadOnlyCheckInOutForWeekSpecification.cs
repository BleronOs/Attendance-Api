using System;
using AttendanceTracker.Core.Entities;
using Ardalis.Specification;
namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyCheckInOutForWeekSpecification : Specification<Check>
    {
        public ReadOnlyCheckInOutForWeekSpecification(DateTime dateTime, int employeeId)
        {
            var currentDateTime = dateTime;
            var firstTimeOfDay = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0, 0);
            var endOfTheDay = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 23, 59, 59);
            Query
                .Include(s => s.Card)
                .Where(s => s.CheckDateTime >= firstTimeOfDay && s.CheckDateTime <= endOfTheDay && s.Card.EmployeeId == employeeId)
                .AsNoTracking();
        }
    }
}
