namespace AttendanceTracker.Api.ViewModels
{
    public class MonthlyHoursCalculate
    {
        public int TotalMonthlyHourse { get; set; }
        public double TotalCompletedHourse { get; set; }
        public DateTime DayOfMonth { get; set; }
        public double RemainingHours { get; set; }


    }
}



