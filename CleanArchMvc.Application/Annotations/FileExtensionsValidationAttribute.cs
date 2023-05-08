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
            IFormFile file = value as IFormFile;

            if (file != null)
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
