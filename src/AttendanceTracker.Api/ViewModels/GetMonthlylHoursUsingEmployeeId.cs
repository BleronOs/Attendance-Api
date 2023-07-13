using System;
namespace AttendanceTracker.Api.ViewModels
{
	public class GetMonthlylHoursUsingEmployeeId
	{
		public double CompletedHours { get; set; }

		public int TargetHours { get; set; }

		public DateTime CurrentDate { get; set; }
	}
}

