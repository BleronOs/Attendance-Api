using System;
using AttendanceTracker.Core.Entities;
namespace AttendanceTracker.Core.ResultModels
{
	public class ResultUser
	{

		public IReadOnlyList<Employee> Employees{ get; set; }
	}
}

