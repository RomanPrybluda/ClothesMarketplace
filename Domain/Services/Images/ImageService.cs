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
            string path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "images");
            Directory.CreateDirectory(path);
            var validationResult = await _fileValidator.ValidateAsync(file);
            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(path, fileName);
            if (validationResult.IsValid)
            {

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                return $"/images/{fileName}";
            }
            else
            {
                throw new CustomException(CustomExceptionType.InvalidInputData, validationResult.Errors.First().ErrorMessage);
            }
        }

        private async Task<bool> CompressImage(IFormFile imageFile)
        {
            using var imageStream = imageFile.OpenReadStream();
            using var image = await Image.LoadAsync(imageStream);

        }
    }
}
