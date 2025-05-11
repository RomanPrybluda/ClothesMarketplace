using Domain.Abstractions;
using Domain.Common.DTOs;
using Domain.Services.Auth.ExtraServices;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("email")]
    public class EmailController_CreatedForTestiong : ControllerBase
    {
        private readonly IEmailProvider _emailProvider;
        private readonly AuthNotifier _authNotifier;

        public EmailController_CreatedForTestiong(IEmailProvider provider, AuthNotifier authNotifier)
        {
            _emailProvider = provider;
            _authNotifier = authNotifier;
        }

        [HttpPost("send")]
        public IActionResult SendMessage(SendEmailDto request)
        {
            _emailProvider.SendTextEmail(request.Email, request.Name, request.Subject, request.Body);
            return Ok($"Letter was successfully send to {request.Email}");
        }

        [HttpPost("sendHtml")]
        public async Task<IActionResult> SendMessageAsync([FromQuery] string email, string userName)
        {
            var result = await _authNotifier.ConfirmPasswordAsync(email, userName);
            return Ok(result);
        }
    }
}
