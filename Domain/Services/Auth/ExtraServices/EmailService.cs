using Domain.Services.Auth.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace Domain.Services.Auth.ExtraServices;

public class EmailService(IConfiguration _configuration) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpClient = new SmtpClient(_configuration["Email:SmtpServer"])
        {
            Port = int.Parse(_configuration["Email:Port"]),
            Credentials = new NetworkCredential(
                _configuration["Email:Username"],
                _configuration["Email:Password"]
            ),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["Email:From"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(to);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
