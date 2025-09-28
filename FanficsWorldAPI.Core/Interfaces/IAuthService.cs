using FanficsWorldAPI.Common.Dto;

namespace FanficsWorldAPI.Core.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResultDto<string>> LoginAsync(LoginDto loginDto);

        Task<ServiceResultDto> RegisterAsync(RegisterDto registerDto);
    }
}
