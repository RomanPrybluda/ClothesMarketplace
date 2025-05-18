using FluentValidation;

namespace Domain.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDTO>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must be less than 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(500)
                .WithMessage("Description must be less than 500 characters.");

            RuleFor(x => x.DollarPrice)
                .NotEmpty()
                .WithMessage("Price is required")
                .Must(x => x > 0);

            RuleFor(x => x.Images)
                .Must(x => x.Count >= 3)
                .WithMessage("At least 3 images is required.")
                .Must(x => x.Count <= 10)
                .WithMessage("You can upload a maximum of 10 images.")
                .ForEach(x =>
                {
                    x.SetValidator(new ImageValidator());
                });

            RuleFor(x => x.MainImageIndex)
                .NotEmpty()
                .WithMessage("Main image index is required.")
                .Must(x => x >= 0)
                .WithMessage("Main image index must be greater than or equal to 0.")
                .Must((x, y) => y < x.Images.Count)
                .WithMessage("Main image index must be less than the number of images.");

            RuleFor(x => x.BrandId)
                .NotEmpty()
                .WithMessage("Brand ID is required.");

            RuleFor(x => x.ColorId)
                .NotEmpty()
                .WithMessage("Color ID is required.");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Category ID is required.");

            RuleFor(x => x.ForWhomId)
                .NotEmpty()
                .WithMessage("For whom ID is required.");

            RuleFor(x => x.ProductConditionId)
                .NotEmpty()
                .WithMessage("Product condition ID is required.");
        }
    }
}
