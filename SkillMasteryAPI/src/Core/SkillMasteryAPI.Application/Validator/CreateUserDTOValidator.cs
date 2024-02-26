
using FluentValidation;
using SkillMasteryAPI.Application.DTOs.User;

namespace SkillMasteryAPI.Application.Validator
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDTO>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .Length(1, 50).WithMessage("The name must be between 1 and 50 characters."); 
            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .Length(1, 50).WithMessage("The name must be between 1 and 50 characters."); 
        
            RuleFor(u => u.Email).NotEmpty().WithMessage("Email is required");
        }
    }
}
