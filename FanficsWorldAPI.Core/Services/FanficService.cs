using FanficsWorldAPI.Common.Dto;
using FanficsWorldAPI.Common.Extensions;
using FanficsWorldAPI.Core.Interfaces;
using FanficsWorldAPI.DataAccess;
using FanficsWorldAPI.DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FanficsWorldAPI.Core.Services
{
    public class FanficService : IFanficService
    {
        private readonly IRepository<Fanfic> _fanficRepository;
        private readonly IValidator<CreateFanficDto> _createFanficDtoValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FanficService(
            IRepository<Fanfic> fanficRepository,
            IValidator<CreateFanficDto> createFanficDtoValidator,
            IHttpContextAccessor httpContextAccessor)
        {
            _fanficRepository = fanficRepository;
            _createFanficDtoValidator = createFanficDtoValidator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResultDto<string>> CreateFanficAsync(CreateFanficDto createFanficDto)
        {
            await _createFanficDtoValidator.ValidateAndThrowAsync(createFanficDto);

            var userId = _httpContextAccessor.HttpContext.User.GetUserId();

            var fanfic = new Fanfic
            {
                Title = createFanficDto.Title,
                Description = createFanficDto.Description,
                Direction = createFanficDto.Direction,
                Rating = createFanficDto.Rating,
                Status = Common.Enums.FanficStatus.InProgress,
                AuthorId = userId,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow
            };

            var created = await _fanficRepository.InsertAsync(fanfic);

            return new ServiceResultDto<string>(
                isSuccess: created,
                value: created ? fanfic.Id : null,
                errors: created ? [] : ["Failed to create a fanfic."]);
        }
    }
}
