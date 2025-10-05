using FanficsWorldAPI.Common.Dto;
using FanficsWorldAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FanficsWorldAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FanficChaptersController : ControllerBase
    {
        private readonly IFanficChapterService _chapterService;

        public FanficChaptersController(IFanficChapterService chapterService)
        {
            _chapterService = chapterService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateChapter(CreateChapterDto createChapterDto)
        {
            var creationResult = await _chapterService.CreateChapterAsync(createChapterDto);

            return creationResult.IsSuccess
                ? StatusCode(StatusCodes.Status201Created)
                : Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Instance = HttpContext.Request.Path,
                    Title = "Failed to create a chapter."
                });
        }
    }
}
