using FanficsWorldAPI.Common.Dto;

namespace FanficsWorldAPI.Core.Interfaces
{
    public interface IFanficChapterService
    {
        Task<ServiceResultDto> CreateChapterAsync(CreateChapterDto createChapterDto);
        Task<ServiceResultDto> DeleteChaptersByFanficIdAsync(string id);
    }
}
