using FanficsWorldAPI.Common.Dto;
using FanficsWorldAPI.DataAccess;
using FanficsWorldAPI.DataAccess.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorldAPI.Core.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator(IRepository<User> userRepository)
        {
            RuleFor(dto => dto.Name)
                .NotEmpty()
                .Length(4, 50)
                .MustAsync(async (name, token) => await BeAUniqueUserNameAsync(name, userRepository, token))
                .WithMessage("User name should be unique. Please choose a different one.");

            RuleFor(dto => dto.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255)
                .MustAsync(async (name, token) => await BeAUniqueEmailAsync(name, userRepository, token))
                .WithMessage("There is already a user registered for this email address. Please choose a different one.");

            RuleFor(dto => dto.Password)
                .NotEmpty()
                .Length(8, 20)
                .Matches(@"[A-z0-9<>?:/$#@!&*()\[\]\{\}\\]*")
                .WithMessage("Password can contain only symbols: A-Z, a-z, 0-9, <, >, ?, :, /, $, #, @, !, &, *, (, ), [, ], {, }, \\");

            RuleFor(dto => dto.BirthDate)
                .NotEmpty()
                .LessThan(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Please specify an actual birth date.");

            When(dto => !string.IsNullOrWhiteSpace(dto.Bio), () =>
            {
                RuleFor(dto => dto.Bio)
                    .MaximumLength(1000);
            });
        }

        private static async Task<bool> BeAUniqueUserNameAsync(string name, IRepository<User> userRepository, CancellationToken cancellationToken) =>
            !await userRepository.EntitySet.AnyAsync(x => x.Name == name, cancellationToken);

        private static async Task<bool> BeAUniqueEmailAsync(string email, IRepository<User> userRepository, CancellationToken cancellationToken) =>
            !await userRepository.EntitySet.AnyAsync(x => x.Email == email, cancellationToken);
    }
}
