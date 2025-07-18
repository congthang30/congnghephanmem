using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EasyBuy.Services.EMAILOTP;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpHost = _configuration["Smtp:Host"];
        var smtpPort = int.Parse(_configuration["Smtp:Port"]);
        var smtpUser = _configuration["Smtp:User"];
        var smtpPass = _configuration["Smtp:Pass"];
        var smtpFrom = _configuration["Smtp:From"];

        using (var client = new SmtpClient(smtpHost, smtpPort))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(smtpUser, smtpPass);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpFrom),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
