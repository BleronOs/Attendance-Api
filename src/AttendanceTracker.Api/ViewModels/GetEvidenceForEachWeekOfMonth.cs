using System;
namespace AttendanceTracker.Api.ViewModels
{
	public class GetEvidenceForEachWeekOfMonth
	{
		public double WeekTarget { get; set; }
		public double DoneHours { get; set; }
		public bool Status { get; set; }
	}
}

