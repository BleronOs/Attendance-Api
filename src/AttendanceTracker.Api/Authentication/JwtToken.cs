namespace AttendanceTracker.Api.Authentication;

public class JwtToken
{
    public string Token { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string RefreshToken { get; set; }

    //per pjesen ku dergohet ne front check in dhe check out

    //public List<DateTime> CheckinoutDates { get; set; }
    //public DateTime Checkin {
    //    get { return CheckinoutDates.OrderBy(s => s).FirstOrDefault(); }
    //}
    //public DateTime Checkout
    //{
    //    get { return CheckinoutDates.OrderByDescending(s => s).FirstOrDefault(); }
    //}
}