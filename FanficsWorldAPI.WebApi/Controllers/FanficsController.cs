using FanficsWorldAPI.Common.Dto;
using FanficsWorldAPI.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorldAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FanficsController : ControllerBase
    {
        private readonly IFanficService _fanficService;

        public FanficsController(IFanficService fanficService)
        {
            _fanficService = fanficService;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateFanfic(CreateFanficDto createFanficDto)
        {
            var creationResult = await _fanficService.CreateFanficAsync(createFanficDto);

            return creationResult.IsSuccess
                ? StatusCode(StatusCodes.Status201Created, creationResult.Value)
                : Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Instance = HttpContext.Request.Path,
                    Title = "Failed to create a new fanfic."
                });
        }
    }
}
