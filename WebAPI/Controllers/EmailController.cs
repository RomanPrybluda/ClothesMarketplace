using Domain.Abstractions;
using Domain.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("email")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailProvider _emailProvider;

        public EmailController(IEmailProvider provider)
        {
            _emailProvider = provider;
        }

        [HttpPost("send")]
        public IActionResult SendMessage(SendEmailDto request)
        {
            _emailProvider.SendEmail(request.Email, request.Name, request.Subject, request.Body);
            return Ok($"Letter was successfully send to {request.Email}");
        }
    }
}
