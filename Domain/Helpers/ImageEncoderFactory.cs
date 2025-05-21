using Domain.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;

namespace Domain
{
    public class ImageEncoderFactory : IImageEncoderFactory
    {
        private readonly ImageEncoderSettings _settings;

        public ImageEncoderFactory(IOptions<ImageEncoderSettings> settings)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            if (_settings.Formats == null || !_settings.Formats.Any())
            {
                throw new InvalidOperationException($"Configuration section '{nameof(ImageEncoderSettings)}.{nameof(ImageEncoderSettings.Formats)}' is missing or empty.");
            }
        }

        public IImageEncoder GetEncoder(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var format = _settings.Formats.FirstOrDefault(f => f.Extensions.Contains(extension));
            if (format == null)
            {
                throw new NotSupportedException($"Unsupported file extension: {extension}");
            }

            return format.Name switch
            {
                nameof(JpegEncoder) => new JpegEncoder
                {
                    Quality = format.Settings.Quality
                },
                nameof(PngEncoder) => new PngEncoder
                {
                    CompressionLevel = ValidatePngCompressionLevel(format.Settings.CompressionLevel)
                },
                nameof(WebpEncoder) => new WebpEncoder
                {
                    Quality = format.Settings.Quality
                },
                _ => throw new NotSupportedException($"Unknown format: {format.Name} in configuration section '{nameof(ImageEncoderSettings)}'")
            };
        }

        private static PngCompressionLevel ValidatePngCompressionLevel(int compressionLevel)
        {
            if (compressionLevel < 0 || compressionLevel > 9)
            {
                return PngCompressionLevel.BestCompression;
            }
            return (PngCompressionLevel)compressionLevel;
        }
    }
}