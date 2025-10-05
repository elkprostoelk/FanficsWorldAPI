using FanficsWorldAPI.Common.Dto;
using FluentValidation;

namespace FanficsWorldAPI.Core.Validators
{
    public class CreateFanficDtoValidator : AbstractValidator<CreateFanficDto>
    {
        public CreateFanficDtoValidator()
        {
            RuleFor(dto => dto.Title)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(dto => dto.Description)
                .MaximumLength(1000);

            RuleFor(dto => dto.Direction)
                .NotNull()
                .IsInEnum();

            RuleFor(dto => dto.Rating)
                .NotNull()
                .IsInEnum();
        }
    }
}
