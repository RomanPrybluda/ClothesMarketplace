using Domain.Abstractions;
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
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" => new JpegEncoder { Quality = 75},
                ".jpeg" => new JpegEncoder { Quality = 75},
                ".png" => new PngEncoder { CompressionLevel = PngCompressionLevel.BestCompression},
                ".webp" => new WebpEncoder()
            };
        }
    }
}
