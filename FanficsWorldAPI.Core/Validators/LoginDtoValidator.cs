using FanficsWorldAPI.Common.Dto;
using FanficsWorldAPI.DataAccess;
using FanficsWorldAPI.DataAccess.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorldAPI.Core.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator(IRepository<User> userRepository)
        {
            RuleFor(dto => dto.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255)
                .MustAsync(async (email, token) => await UserExistsAsync(email, userRepository, token))
                .WithMessage("User does not exist.");

            RuleFor(dto => dto.Password)
                .NotEmpty()
                .Length(8, 20)
                .Matches(@"[A-z0-9<>?:/$#@!&*()\[\]\{\}\\]*")
                .WithMessage("Password can contain only symbols: A-Z, a-z, 0-9, <, >, ?, :, /, $, #, @, !, &, *, (, ), [, ], {, }, \\");
        }

        private static async Task<bool> UserExistsAsync(
            string email,
            IRepository<User> userRepository,
            CancellationToken cancellationToken) => await userRepository.EntitySet
                .AnyAsync(x => x.Email == email, cancellationToken);
    }
}
