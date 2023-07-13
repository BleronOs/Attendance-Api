using System;
namespace AttendanceTracker.Api.ViewModels
{
	public class WeekEvidenceStatistice
	{
			public int ActiveEmployee  { get; set; }
		    public int PassiveEmployee { get; set; }
		    public string DayOfWeeks { get; set; }
	     	public double HoursDay { get; set; }

    }
}

