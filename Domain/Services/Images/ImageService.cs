using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Images
{
    public class ImageService
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IValidator<IFormFile> _fileValidator;

        public ImageService(IHostEnvironment environment, IValidator<IFormFile> fileValidator)
        {
            _hostEnvironment = environment;
            _fileValidator = fileValidator;
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            var validationResult = _fileValidator.Validate(file);
            if (validationResult.IsValid)
            {

            }
        }
    }
}
