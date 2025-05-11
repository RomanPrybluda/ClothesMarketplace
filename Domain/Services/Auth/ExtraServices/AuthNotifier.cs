using ClothesMarketPlace.Infrastructure.Helpers;
using Domain.Abstractions;
using Domain.Сommon.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Auth.ExtraServices
{
    public class AuthNotifier(IEmailProvider emailProvider, IHtmlTemplatesReader reader)
    {
        public async Task<string> ConfirmPasswordAsync(string email, string receiverName)
        {
            var content = await reader.ReadAsync("confirm_email_template.html");
            var result = emailProvider.SendHtmlEmail(email, receiverName, "Confirm email", content);

            if (result == null)
                throw new CustomException(CustomExceptionType.InternalError, 
                    "We're sorry, but the email could not be sent at this moment due to a technical issue. Please try again later. ");
            return "Your email has been successfully delivered to the recipient's inbox. Thank you for using our service!";
        }
    }
}
