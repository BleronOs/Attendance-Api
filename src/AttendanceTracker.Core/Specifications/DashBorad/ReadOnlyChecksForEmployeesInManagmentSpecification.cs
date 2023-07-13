using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
    public class ReadOnlyChecksForEmployeesInManagmentSpecification : Specification<Check>
    {
        public ReadOnlyChecksForEmployeesInManagmentSpecification(int managerId, DateTime dateTime)
        {
            var currentDateTime = dateTime;
            var firstTimeOfDay = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0, 0);
            var endOfTheDay = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 23, 59, 59);
            Query
                .Where(s => s.CheckDateTime >= firstTimeOfDay && s.CheckDateTime <= endOfTheDay)
                .Include(u => u.User)
                .Include(s => s.Card)
                .ThenInclude(e => e.Employee)
                .ThenInclude(em => em.EmployeeManagment)
                .ThenInclude(m => m.Manager)
                .Where(s => s.Card.Employee.EmployeeManagment.Manager.EmployeeId == managerId).AsNoTracking();
        }
    }
}


