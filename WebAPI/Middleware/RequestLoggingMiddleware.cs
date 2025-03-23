using Microsoft.Extensions.Options;

namespace WebAPI
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly string _logText;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger,
            IOptions<RequestLoggingSettings> options)
        {
            _next = next;
            _logger = logger;
            _logText = options.Value.Text;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("{_logText}: {date} {time}.", _logText, DateTime.UtcNow.ToString("MM-dd-yyyy"), DateTime.UtcNow.ToString("HH:mm:ss"));
            await _next(context);
        }
    }

}
