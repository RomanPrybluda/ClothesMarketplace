using Domain;

namespace WebAPI
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly string _internalErrorMessage = "Something went wrong";

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger,
            IWebHostEnvironment env
            )
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (Exception ex)
            {
                if (ex is CustomException exception)
                    await LogShipInfoException(context, exception);
                else
                    await LogUnhandledException(context, ex);
            }
        }

        private async Task LogShipInfoException(HttpContext context, CustomException ex)
        {
            _logger.LogError($"{DateTime.UtcNow.ToString("MM-dd-yyyy HH:mm:ss")} {ex.ExceptionType}  {ex.Message} {ex.StackTrace}");

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsync(ex.ToErrorResponse().Serialize());
        }

        private async Task LogUnhandledException(HttpContext context, Exception ex)
        {
            _logger.LogError($"{DateTime.UtcNow.ToString("MM-dd-yyyy HH:mm:ss")} {ex.Message} {ex.StackTrace}");

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            ErrorResponse errorResponse;

            if (_env.IsProduction())
                errorResponse = new ErrorResponse(CustomExceptionType.InternalError.ToString(), _internalErrorMessage);
            else
                errorResponse = new ErrorResponse(CustomExceptionType.InternalError.ToString(), $"{ex.Message} {ex.StackTrace}");

            await context.Response.WriteAsync(errorResponse.Serialize());
        }
    }
}
