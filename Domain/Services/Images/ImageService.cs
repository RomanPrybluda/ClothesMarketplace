﻿using Domain.Abstractions;
using Domain.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;

namespace Domain.Services.Images
{
    public class ImageService : IImageService
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IValidator<IFormFile> _fileValidator;
        private readonly IImageEncoderFactory _encoderFactory;
        private readonly IConfiguration _configuration;

        public ImageService(IHostEnvironment environment, IValidator<IFormFile> fileValidator, 
            IImageEncoderFactory encoderFactory, IConfiguration configuration)
        {
            _hostEnvironment = environment;
            _fileValidator = fileValidator;
            _encoderFactory = encoderFactory;
            _configuration = configuration;
        }

        public async Task<List<string>> UploadMultipleImagesAsync(List<IFormFile> imageFiles)
        {
            var urlsList = new List<string>();
            foreach (var imageFile in imageFiles)
            {
                string imageUrl = await UploadImageAsync(imageFile);
                urlsList.Add(imageUrl);
            }
            return urlsList;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var validationResult = await _fileValidator.ValidateAsync(file);
            if (validationResult.IsValid)
            {
                string baseUrl = _configuration["ImageBaseUrl"];
                var compressedContent = await CompressImage(file);
                var uniqueFileName = GenerateUniqueImageName(file.FileName);
                var fullUrl = Path.Combine(baseUrl, uniqueFileName);
                var webpImage = ImageConverter.ConvertToWebpImageFormat(compressedContent);
                string fileNameWithoutExtension = uniqueFileName.Split('.')[0];
                await File.WriteAllBytesAsync(fullUrl, webpImage);
                return fileNameWithoutExtension;
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
            image.Metadata.IptcProfile = null;
            image.Metadata.XmpProfile = null;
            image.Metadata.IccProfile = null;
            image.Metadata.CicpProfile = null;
            await image.SaveAsync(memoryStream, encoder);
            return memoryStream.ToArray();
        }

        private string GenerateUniqueImageName(string fileName)
        {
            var fileExtension = ".webp";
            string newFileName = Guid.NewGuid().ToString("N");
            var uniqueFileName = newFileName + fileExtension;
            return uniqueFileName;
        }



    }
}
