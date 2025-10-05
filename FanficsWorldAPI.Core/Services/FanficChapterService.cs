using FanficsWorldAPI.Common.Dto;
using FanficsWorldAPI.Core.Interfaces;
using FanficsWorldAPI.DataAccess;
using FanficsWorldAPI.DataAccess.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FanficsWorldAPI.Core.Services
{
    public class FanficChapterService : IFanficChapterService
    {
        private readonly IRepository<FanficChapter> _fanficChapterRepository;
        private readonly IValidator<CreateChapterDto> _createChapterDtoValidator;

        public FanficChapterService(
            IRepository<FanficChapter> fanficChapterRepository,
            IValidator<CreateChapterDto> createChapterDtoValidator)
        {
            _fanficChapterRepository = fanficChapterRepository;
            _createChapterDtoValidator = createChapterDtoValidator;
        }

        public async Task<ServiceResultDto> CreateChapterAsync(CreateChapterDto createChapterDto)
        {
            await _createChapterDtoValidator.ValidateAndThrowAsync(createChapterDto);

            var lastChapterNumber = await _fanficChapterRepository
                .EntitySet
                .Where(x => x.FanficId == createChapterDto.FanficId)
                .OrderByDescending(x => x.Number)
                .Select(x => x.Number)
                .FirstOrDefaultAsync();

            var chapter = new FanficChapter
            {
                Title = createChapterDto.Title,
                Description = createChapterDto.Description,
                IsDraft = createChapterDto.IsDraft,
                TextHtml = createChapterDto.TextHtml,
                FanficId = createChapterDto.FanficId,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                Number = ++lastChapterNumber
            };

            var created = await _fanficChapterRepository.InsertAsync(chapter);
            return new ServiceResultDto(created, created ? [] : ["Failed to create a chapter."]);
        }
    }
}
