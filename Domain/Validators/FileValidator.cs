using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators
{
    public class FileValidator :AbstractValidator<IFormFile>
    {
        public FileValidator()
        {
            RuleFor(file => file.Length).LessThanOrEqualTo(5242880).WithMessage("File size must be less than or equal to 5 MB.");
            RuleFor(file => Path.GetExtension(file.Name)).Matches(@"\.(jpg|jpeg|png|webp)$").WithMessage("File type must be jpeg, jpg, png or webp.");
            RuleFor(file => file).NotNull().WithMessage("At least one image is required.");
        }
    }
}
