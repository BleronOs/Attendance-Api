namespace AttendanceTracker.Infrastructure.Configuration.Email;

public class EmailConfiguration
{
    public int Port { get; set; }
    public string SmtpServer { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}