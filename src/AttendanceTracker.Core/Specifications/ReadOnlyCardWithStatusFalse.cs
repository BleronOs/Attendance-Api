using System;
using AttendanceTracker.Core.Entities;
using Ardalis.Specification;
namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyCardWithStatusFalse:Specification<Card>
	{
		public ReadOnlyCardWithStatusFalse()
		{
            Query.Include(e => e.Employee)
                 .Where(e => e.Status == false).AsNoTracking();
        }
	}
}

