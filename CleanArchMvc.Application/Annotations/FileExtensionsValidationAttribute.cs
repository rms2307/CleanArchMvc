using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace CleanArchMvc.Application.Annotations
{
    public class FileExtensionsValidationAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public FileExtensionsValidationAttribute(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var fileExtension = Path.GetExtension(file.FileName);

                if (!_allowedExtensions.Contains(fileExtension.ToLower()))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
