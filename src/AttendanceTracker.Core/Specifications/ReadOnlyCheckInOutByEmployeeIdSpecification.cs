using System;
using AttendanceTracker.Core.Entities;
using Ardalis.Specification;
namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyCheckInOutByEmployeeIdSpecification : Specification<Check>
    {
        public ReadOnlyCheckInOutByEmployeeIdSpecification(DateTime dateTime, int employeeId)
        {
            var currentDateTime = dateTime;
            var firstTimeOfDay = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0, 0);
            var endOfTheDay = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 23, 59, 59);

            Query
                .Where(s => s.CheckDateTime >= firstTimeOfDay && s.CheckDateTime <= endOfTheDay)
                .Include(s => s.Card)
                .ThenInclude(e => e.Employee)
                .ThenInclude(em => em.EmployeeManagment)
                .ThenInclude(m => m.Manager)
                .Where(s => s.Card.Employee.EmployeeManagment.Manager.EmployeeId == employeeId).AsNoTracking();
        }
    }
}
