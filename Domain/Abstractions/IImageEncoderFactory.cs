using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats;

namespace Domain.Abstractions
{
    public interface IImageEncoderFactory
    {
        IImageEncoder GetEncoder(IFormFile file);
    }
}