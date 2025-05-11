using Domain.Abstractions;
using Domain.Common.DTOs;
using Microsoft.Extensions.Options;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesMarketPlace.Infrastructure.Services.Email
{
    public class EmailProvider : IEmailProvider
    {
        private readonly BrevoApiSettings _settings;

        public EmailProvider(IOptions<BrevoApiSettings> brevoSettings)
        {
            _settings = brevoSettings.Value;
        }

        public string SendTextEmail(string receiverEmail, string receiverName, string subject, string text)
        {
            var config = new Configuration();
            config.AddApiKey("api-key", _settings.ApiKey);
            var apiInstance = new TransactionalEmailsApi(config);
            SendSmtpEmailSender sender = new SendSmtpEmailSender(_settings.SenderName, _settings.SenderEmail);

            SendSmtpEmailTo receiver = new SendSmtpEmailTo(receiverEmail, receiverName);
            List<SendSmtpEmailTo> sendSmtpEmailTos = new List<SendSmtpEmailTo>();
            sendSmtpEmailTos.Add(receiver);

            string htmlContent = null;
            string textContent = text;

            var sendSmtpEmail = new SendSmtpEmail(sender, sendSmtpEmailTos, null, null, htmlContent, textContent, subject);
            CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);

            return result.MessageId;
        }

        public string SendHtmlEmail(string receiverEmail, string receiverName, string subject, string htmlContent)
        {
            var config = new Configuration();
            config.AddApiKey("api-key", _settings.ApiKey);
            var apiInstance = new TransactionalEmailsApi(config);
            SendSmtpEmailSender sender = new SendSmtpEmailSender(_settings.SenderName, _settings.SenderEmail);

            SendSmtpEmailTo receiver = new SendSmtpEmailTo(receiverEmail, receiverName);
            List<SendSmtpEmailTo> sendSmtpEmailTos = new List<SendSmtpEmailTo>();
            sendSmtpEmailTos.Add(receiver);

            var sendSmtpEmail = new SendSmtpEmail(sender, sendSmtpEmailTos, null, null, htmlContent, null, subject);
            CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
            
            return result.MessageId;
        }
    }
}
