using Domain.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Images
{
    public class ImageService : IImageService
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IValidator<IFormFile> _fileValidator;
        private readonly IImageEncoderFactory _encoderFactory;

        public ImageService(IHostEnvironment environment, IValidator<IFormFile> fileValidator, IImageEncoderFactory encoderFactory)
        {
            _hostEnvironment = environment;
            _fileValidator = fileValidator;
            _encoderFactory = encoderFactory;
        }

        public async Task<string> UploadImageFileAsync(IFormFile file)
        {
            var validationResult = await _fileValidator.ValidateAsync(file);
            if (validationResult.IsValid)
            {
                string path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images");
                var compressedContent = await CompressImage(file);
                Directory.CreateDirectory(path);
                var uniqueFileName = GenerateUniqueImageName(file.FileName);
                var filePath = Path.Combine(path, uniqueFileName);
                await File.WriteAllBytesAsync(filePath, compressedContent);
                return filePath;
            }
            else
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"File validation failed: {errors}");
            }

        }

        private async Task<byte[]> CompressImage(IFormFile imageFile)
        {
            using var imageStream = imageFile.OpenReadStream();
            using var image = await Image.LoadAsync(imageStream);
            using var memoryStream = new MemoryStream();
            var encoder = _encoderFactory.GetEncoder(imageFile);
            await image.SaveAsync(memoryStream, encoder);
            return memoryStream.ToArray();
        }

        private string GenerateUniqueImageName(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var uniqueFileName = Guid.NewGuid().ToString("N") + fileExtension;
            return uniqueFileName;
        }
    }
}
