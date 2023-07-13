using System;
namespace AttendanceTracker.Api.ViewModels
{
    public class TodayCheksResponeseCalculated
    {
            public string CardNumber { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public DateTime FirstDateTime { get; set; }
            public DateTime? LastDateTime { get; set; }
            public double CompletedWeekHours { get; set; }
            public double TotalWeekHours { get; set; }
	}
}

