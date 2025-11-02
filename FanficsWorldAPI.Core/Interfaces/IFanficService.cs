using FanficsWorldAPI.Common.Dto;

namespace FanficsWorldAPI.Core.Interfaces
{
    public interface IFanficService
    {
        Task<ServiceResultDto<string>> CreateFanficAsync(CreateFanficDto createFanficDto);
        Task<PagedResultDto<FanficListItemDto>> SearchForFanficsAsync(
            SearchFanficsDto searchFanficsDto,
            CancellationToken cancellationToken);
    }
}
