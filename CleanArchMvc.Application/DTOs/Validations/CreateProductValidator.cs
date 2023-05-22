using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace CleanArchMvc.Application.DTOs.Validations
{
    public class CreateProductValidator : AbstractValidator<CreateProductDTO>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(3)
                .MaximumLength(100)
                .NotEmpty();

            RuleFor(x => x.Description)
                .MinimumLength(5)
                .MaximumLength(200)
                .NotEmpty();

            RuleFor(x => x.Price)
                .PrecisionScale(18, 2, true)
                .NotNull();

            RuleFor(x => x.Stock)   
                .InclusiveBetween(1, 9999)
                .NotNull();

            RuleFor(x => x.Image).Custom((image, context) =>
            {
                ValidateFileSize(image, context);
            });

            RuleFor(x => x.Image).Custom((image, context) =>
            {
                ValidateFileExtensions(image, context);
            });
        }

        private static void ValidateFileSize(IFormFile image, ValidationContext<CreateProductDTO> context)
        {
            int maxImageSize = 1024 * 1024;
            if (image != null && image.Length > maxImageSize)
                context.AddFailure("Image size should be less than 1 MB.");
        }

        private static void ValidateFileExtensions(IFormFile image, ValidationContext<CreateProductDTO> context)
        {
            string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (image is IFormFile file)
            {
                var fileExtension = Path.GetExtension(file.FileName);

                if (!allowedExtensions.Contains(fileExtension.ToLower()))
                    context.AddFailure($"Only {allowedExtensions} files are allowed.");
            }
        }
    }
}
