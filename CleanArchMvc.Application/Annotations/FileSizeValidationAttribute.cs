using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CleanArchMvc.Application.Annotations
{
    public class FileSizeValidationAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public FileSizeValidationAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile file = value as IFormFile;

            if (file != null && file.Length > _maxFileSize)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
