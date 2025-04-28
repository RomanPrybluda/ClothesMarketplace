using Domain.Сommon.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Extentions
{
    public static class ResultExtensions
    {
        public static IActionResult ToResponse<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result.Value);
            return new BadRequestObjectResult(result.ToErrorResponse());
        }
    }
}
