using FanficsWorldAPI.Common.Constants;
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

        [HttpGet]
        [ProducesResponseType<PagedResultDto<FanficListItemDto>>(StatusCodes.Status200OK, "application/json")]
        public async Task<IActionResult> SearchFanfics(
            [FromQuery] SearchFanficsDto searchFanficsDto,
            CancellationToken cancellationToken)
        {
            return Ok(await _fanficService.SearchForFanficsAsync(searchFanficsDto, cancellationToken));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict, "application/json")]
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

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict, "application/json")]
        public async Task<IActionResult> DeleteFanfic(string id)
        {
            var deleteResult = await _fanficService.DeleteFanficAsync(id);
            
            if (deleteResult.IsSuccess)
            {
                return NoContent();
            }

            if (deleteResult.Errors.Contains(ErrorMessageConstants.FanficNotFound))
            {
                return NotFound();
            }

            if (deleteResult.Errors.Contains(ErrorMessageConstants.Forbidden))
            {
                return Forbid();
            }

            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Instance = HttpContext.Request.Path,
                Title = "Failed to delete the fanfic."
            });
        }
    }
}
