using FanficsWorldAPI.Common.Dto;
using FanficsWorldAPI.DataAccess;
using FanficsWorldAPI.DataAccess.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorldAPI.Core.Validators
{
    public class CreateChapterDtoValidator : AbstractValidator<CreateChapterDto>
    {
        public CreateChapterDtoValidator(IRepository<Fanfic> fanficRepository)
        {
            RuleFor(dto => dto.FanficId)
                .NotEmpty()
                .MustAsync(async (id, token) => await ShouldFanficExistAsync(id, fanficRepository, token))
                .WithMessage("Fanfic with such ID does not exist.");

            RuleFor(dto => dto.IsDraft)
                .NotNull();

            When(dto => !string.IsNullOrWhiteSpace(dto.Title), () =>
            {
                RuleFor(dto => dto.Title)
                    .MaximumLength(100);
            });

            When(dto => !string.IsNullOrWhiteSpace(dto.Description), () =>
            {
                RuleFor(dto => dto.Description)
                    .MaximumLength(1000);
            });

            RuleFor(dto => dto.TextHtml)
                .NotEmpty();
        }

        private static async Task<bool> ShouldFanficExistAsync(
            string id,
            IRepository<Fanfic> fanficRepository,
            CancellationToken token) => await fanficRepository.EntitySet.AnyAsync(x => x.Id == id, token);
    }
}
