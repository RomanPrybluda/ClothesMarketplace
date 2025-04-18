﻿using System.Net;
using System.Net.Mail;

namespace Domain;

public class EmailService()
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
