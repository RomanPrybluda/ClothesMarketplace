using Domain.Abstractions;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public class ImageEncoderFactory: IImageEncoderFactory
    {
        public IImageEncoder GetEncoder(IFormFile file)
        {
            var extension = Path.GetExtension(file.Name).ToLowerInvariant();
            return extension switch
            {
                ".jpg" => new JpegEncoder(),
                ".jpeg" => new JpegEncoder(),
                ".png" => new PngEncoder(),
                ".webp" => new WebpEncoder(),
                _ => throw new NotSupportedImageFormatException($"Unsupported image format: {extension}")
            };
        }
    }
}
