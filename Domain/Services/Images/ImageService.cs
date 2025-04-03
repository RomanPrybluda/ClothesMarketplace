using Domain.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
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
    public class ImageService
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
                var compressedFile = await CompressImage(file);
                Directory.CreateDirectory(path);
                var filePath = Path.Combine(path, compressedFile.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                return $"/images/{compressedFile.FileName}";
            }
            else
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"File validation failed: {errors}");
            }

        }

        private async Task<IFormFile> CompressImage(IFormFile imageFile)
        {
            using var imageStream = imageFile.OpenReadStream();
            using var image = await Image.LoadAsync(imageStream);
            using var memoryStream = new MemoryStream();
            var encoder = _encoderFactory.GetEncoder(imageFile);
            await image.SaveAsync(memoryStream, encoder);
            memoryStream.Position = 0;
            var imageName = GenerateUniqueImageName(imageFile.FileName);
            return new FormFile(memoryStream, 0, memoryStream.Length, imageName, imageName);
        }

        private string GenerateUniqueImageName(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var uniqueFileName = Path.GetRandomFileName() + fileExtension;
            return uniqueFileName;
        }
    }
}
