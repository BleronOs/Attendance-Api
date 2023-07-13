using System;
namespace AttendanceTracker.Api.ViewModels{
public class CalculatedHours
{
    public double CompletedHoursPerDay { get; set; }
    public double TotalHoursPerDay { get; set; }
    public DateTime dateTime { get; set; }
}
}