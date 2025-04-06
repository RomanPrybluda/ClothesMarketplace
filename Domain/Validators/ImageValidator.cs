using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators
{
    public class ImageValidator :AbstractValidator<IFormFile>
    {
        public ImageValidator()
        {
            RuleFor(file => file.Length)
                .LessThanOrEqualTo(1048576)
                .WithMessage("Image size must be less than 1 MB.");
            RuleFor(file => Path.GetExtension(file.FileName))
                .Matches(@"\.(jpg|jpeg|png|webp)$")
                .WithMessage("File type must be jpeg, jpg, png or webp.");
            RuleFor(file => file)
                .NotNull()
                .WithMessage("The image field cannot be left empty.");
        }
    }
}
