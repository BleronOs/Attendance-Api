using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace AttendanceTracker.Infrastructure.Configuration.Email;

public class EmailSender : IEmailSender
{
    private readonly EmailConfiguration _configuration;

    public EmailSender(EmailConfiguration configuration)
    {
        _configuration = configuration;
    }

    private MimeMessage CreateMimeMessageFromEmailMessage(string fromEmail, string toEmail, string subject, string body, TextFormat format = TextFormat.Text)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(fromEmail, fromEmail));
        mimeMessage.To.Add(new MailboxAddress(toEmail, toEmail));
        mimeMessage.Subject = subject;
        mimeMessage.Body = new TextPart(format) {Text = body};
        return mimeMessage;
    }

        
    public Tuple<bool, string> Send(string fromEmail, string toEmail, string subject, string body, TextFormat format = TextFormat.Html)
    {
        using SmtpClient smtpClient = new SmtpClient(new ProtocolLogger("smtp.log"));
        try
        {
            var message = CreateMimeMessageFromEmailMessage(fromEmail, toEmail, subject, body, format);
            smtpClient.Connect(_configuration.SmtpServer, _configuration.Port, false);
            smtpClient.Authenticate(_configuration.Username, _configuration.Password);
            smtpClient.Send(message);
            smtpClient.Disconnect(true);
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(false, ex.Message);
        }

        return new Tuple<bool, string>(true, "");
    }



    public Tuple<bool, string> Send(string toEmail, string subject, string body, TextFormat format = TextFormat.Html)
    {
        using SmtpClient smtpClient = new SmtpClient(new ProtocolLogger("smtp.log"));
        try
        {
            var message = CreateMimeMessageFromEmailMessage(_configuration.Username, toEmail, subject, body, format);
            smtpClient.Connect(_configuration.SmtpServer, _configuration.Port, false);
            smtpClient.Authenticate(_configuration.Username, _configuration.Password);
            smtpClient.Send(message);
            smtpClient.Disconnect(true);
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(false, ex.Message);
        }

        return new Tuple<bool, string>(true, "");
    }
    public async Task<Tuple<bool, string>> SendAsync(string toEmail, string subject, string body, TextFormat format = TextFormat.Html)
    {
        using SmtpClient smtpClient = new SmtpClient(new ProtocolLogger("smtp.log"));
        try
        {
            var message = CreateMimeMessageFromEmailMessage(_configuration.Username, toEmail, subject, body, format);
            await smtpClient.ConnectAsync(_configuration.SmtpServer, _configuration.Port, false);
            await smtpClient.AuthenticateAsync(_configuration.Username, _configuration.Password);
            await smtpClient.SendAsync(message);
            await smtpClient.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(false, ex.Message);
        }

        return new Tuple<bool, string>(true, "");
    }

}