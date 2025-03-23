using Domain;

namespace WebAPI
{
    public static class Extensions
    {
        public static ErrorResponse ToErrorResponse(this CustomException ex)
        {
            return new ErrorResponse
            {
                Type = ex.ExceptionType.ToString(),
                Description = ex.Message
            };
        }
    }
}
