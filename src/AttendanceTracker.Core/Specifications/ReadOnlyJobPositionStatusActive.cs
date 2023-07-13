using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyJobPositionStatusActive:Specification<JobPosition>
	{
		public ReadOnlyJobPositionStatusActive()
		{
			Query.
				Where(e => e.Status == true).AsNoTracking();
		}
	}
}

