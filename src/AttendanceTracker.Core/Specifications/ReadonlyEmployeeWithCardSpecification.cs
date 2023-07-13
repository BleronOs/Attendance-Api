using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;

namespace AttendanceTracker.Core.Specifications
{
	public sealed class ReadonlyEmployeeWithCardSpecification : Specification<Employee>
	{
		public ReadonlyEmployeeWithCardSpecification()
		{
			Query
				.Include(s => s.Cards)
				.Where(e => e.Cards.Any(a => a.Status))
				.AsNoTracking();
		}
	}
}

