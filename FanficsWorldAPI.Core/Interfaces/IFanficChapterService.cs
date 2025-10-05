using FanficsWorldAPI.Common.Dto;

namespace FanficsWorldAPI.Core.Interfaces
{
    public interface IFanficChapterService
    {
        Task<ServiceResultDto> CreateChapterAsync(CreateChapterDto createChapterDto);
    }
}
