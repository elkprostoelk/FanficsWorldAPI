using FanficsWorldAPI.Common.Dto;
using FanficsWorldAPI.Core.Interfaces;
using FanficsWorldAPI.DataAccess;
using FanficsWorldAPI.DataAccess.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FanficsWorldAPI.Core.Services
{
    public class FanficChapterService : IFanficChapterService
    {
        private readonly IRepository<FanficChapter> _fanficChapterRepository;
        private readonly IValidator<CreateChapterDto> _createChapterDtoValidator;
        private readonly ILogger _logger = Log.ForContext<FanficChapterService>();

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
                Number = lastChapterNumber + 1
            };

            var created = await _fanficChapterRepository.InsertAsync(chapter);
            return new ServiceResultDto(created, created ? [] : ["Failed to create a chapter."]);
        }

        public async Task<ServiceResultDto> DeleteChaptersByFanficIdAsync(string fanficId)
        {
            var chapters = await _fanficChapterRepository
                .EntitySet
                .Where(c => c.FanficId == fanficId)
                .ToListAsync();

            if (chapters.Count == 0)
            {
                _logger.Information("No chapters found for fanfic with ID {FanficId}", fanficId);
                return new ServiceResultDto(true);
            }

            var deleted = await _fanficChapterRepository.DeleteRangeAsync(chapters);
            if (deleted)
            {
                _logger.Information("Deleted all chapters for a fanfic with ID {FanficId}", fanficId);
                return new ServiceResultDto(true);
            }

            _logger.Error("Failed to delete chapters for a fanfic with ID {FanficId}", fanficId);
            return new ServiceResultDto(false);
        }
    }
}
