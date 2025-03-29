using Domain.Services.Auth.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace Domain.Services.Auth.ExtraServices;

public class EmailService(IConfiguration _configuration) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("yourusername@gmail.com", "yourpassword"),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("yourusername@gmail.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(toEmail);

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (SmtpException ex)
        {
            Console.WriteLine("SMTP Error: " + ex.Message);
        }
    }
}
