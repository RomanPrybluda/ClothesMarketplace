using Microsoft.AspNetCore.Http;

namespace Domain.Abstractions
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<List<string>> UploadMultipleImagesAsync(List<IFormFile> imageFiles);
    }
}