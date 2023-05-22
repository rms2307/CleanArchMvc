using FluentValidation;

namespace CleanArchMvc.Application.DTOs.Validations
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDTO>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(3)
                .MaximumLength(100)
                .NotEmpty();
        }
    }
}
