using System;
using Ardalis.Specification;
using AttendanceTracker.Core.Entities;
namespace AttendanceTracker.Core.Specifications
{
	public class ReadOnlyEmployeeWithoutCardAndStatusPasive : Specification<Employee>
	{
		public ReadOnlyEmployeeWithoutCardAndStatusPasive()
		{
			Query
				.Include(e => e.Cards)
				.Where(m => m.Cards == null || !m.Cards.Where(s=>s.Status == true).Any());

        }
	}
}

