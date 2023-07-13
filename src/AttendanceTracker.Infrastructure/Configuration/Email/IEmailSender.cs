using MimeKit.Text;

namespace AttendanceTracker.Infrastructure.Configuration.Email;

public interface IEmailSender
{
    /// <summary>
    /// The Method uses SMTP Client to send an email based on the configurations you pass in the constructor
    /// </summary>
    /// <param name="fromEmail"></param>
    /// <param name="toEmail"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    

    //Tuple<bool, string> Send(string fromEmail, string toEmail, string subject, string body,
    //    TextFormat format = TextFormat.Html);
    Tuple<bool, string> Send(string toEmail, string subject, string body, TextFormat format = TextFormat.Html);
    Task<Tuple<bool, string>> SendAsync(string toEmail, string subject, string body, TextFormat format = TextFormat.Html);
}