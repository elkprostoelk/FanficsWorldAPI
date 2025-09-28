using FanficsWorldAPI.Common.Constants;
using FanficsWorldAPI.Common.Dto;
using FanficsWorldAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorldAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var authResult = await _authService.LoginAsync(loginDto);

            if (authResult.IsSuccess)
            {
                return Ok(authResult.Value);
            }

            if (authResult.Errors.Contains(ErrorMessageConstants.InvalidPassword))
            {
                return Unauthorized(new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Instance = HttpContext.Request.Path,
                    Title = ErrorMessageConstants.InvalidPassword
                });
            }

            return Forbid();
        }
    }
}
