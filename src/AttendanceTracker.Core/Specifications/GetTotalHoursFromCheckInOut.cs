using System;
using AttendanceTracker.Core.Entities;
using Ardalis.Specification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AttendanceTracker.Core.Specifications
{
	public class GetTotalHoursFromCheckInOut : Specification<Check>
	{
		public GetTotalHoursFromCheckInOut(DateTime dateTime)
		{
            var currentDateTime = dateTime;
            var firstTimeOfDay = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0, 0);
            var endOfTheDay = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 23, 59, 59);
            Query
                .Where(s => s.CheckDateTime >= firstTimeOfDay && s.CheckDateTime <= endOfTheDay)
                .Include(s => s.Card)
                .ThenInclude(e => e.Employee).AsNoTracking();
        }
	}
}

