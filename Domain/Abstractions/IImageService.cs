using Microsoft.AspNetCore.Http;

namespace Domain.Abstractions
{
    public interface IImageService
    {
        Task<string> UploadImageFileAsync(IFormFile file);
    }
}