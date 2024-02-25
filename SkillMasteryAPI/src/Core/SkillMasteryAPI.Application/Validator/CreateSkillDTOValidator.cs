
using FluentValidation;
using SkillMasteryAPI.Application.DTOs.Skill;


namespace SkillMasteryAPI.Application.Validator
{
    public class CreateSkillDTOValidator : AbstractValidator<CreateSkillDTO>
    {
        public CreateSkillDTOValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage("First Name is required");
            RuleFor(u => u.Description).NotEmpty().WithMessage("Description is required");

        }
    }
}
