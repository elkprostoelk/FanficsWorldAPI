using FanficsWorldAPI.Common.Constants;
using FanficsWorldAPI.Common.Dto;
using FanficsWorldAPI.Core.Interfaces;
using FanficsWorldAPI.Core.Services;
using FanficsWorldAPI.Core.Validators;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FanficsWorldAPI.UnitTests.Services
{
    public class FanficServiceTests : ServiceTestsBase
    {
        private FanficService _fanficService;

        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
        private readonly Mock<IFanficChapterService> _fanficChapterServiceMock = new();

        public FanficServiceTests() : base()
        {
            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "456")]))
            };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            _fanficService = new FanficService(
                fanficRepository,
                new CreateFanficDtoValidator(),
                _httpContextAccessorMock.Object,
                _fanficChapterServiceMock.Object);
        }

        [Fact]
        public async Task DeleteFanficAsync_NoFanficFound_ReturnsFailure()
        {
            // Arrange

            const string fanficId = "nonexistent-fanfic-id";

            // Act

            var result = await _fanficService.DeleteFanficAsync(fanficId);

            // Assert

            Assert.False(result.IsSuccess);
            Assert.Contains(ErrorMessageConstants.FanficNotFound, result.Errors);
        }

        [Fact]
        public async Task DeleteFanficAsync_OtherAuthorFanfic_ReturnsFailure()
        {
            // Arrange

            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "01KANQD038490G4ZQBP669ABZF")]))
            };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            _fanficService = new FanficService(
                fanficRepository,
                new CreateFanficDtoValidator(),
                _httpContextAccessorMock.Object,
                _fanficChapterServiceMock.Object);

            const string fanficId = "01KANQK7FA9HPY1ASZ9TY241YN";

            // Act

            var result = await _fanficService.DeleteFanficAsync(fanficId);

            // Assert

            Assert.False(result.IsSuccess);
            Assert.Contains(ErrorMessageConstants.Forbidden, result.Errors);
        }

        [Fact]
        public async Task DeleteFanficAsync_ChaptersDeletingFailed_ReturnsFailure()
        {
            // Arrange

            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "01KANQD038490G4ZQBP669ABZF")]))
            };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            _fanficService = new FanficService(
                fanficRepository,
                new CreateFanficDtoValidator(),
                _httpContextAccessorMock.Object,
                _fanficChapterServiceMock.Object);

            const string fanficId = "01KANQK7ESG4P91Z6JRQJT4GGM";

            _fanficChapterServiceMock
                .Setup(x => x.DeleteChaptersByFanficIdAsync(fanficId))
                .ReturnsAsync(new ServiceResultDto(isSuccess: false));

            // Act

            var result = await _fanficService.DeleteFanficAsync(fanficId);

            // Assert

            Assert.False(result.IsSuccess);
            Assert.Contains(ErrorMessageConstants.CannotDeleteFanficChaptersNotDeleted, result.Errors);
        }

        [Fact]
        public async Task DeleteFanficAsync_SuccessfulDeletion()
        {
            // Arrange

            var httpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "01KANQD038490G4ZQBP669ABZF")]))
            };

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            _fanficService = new FanficService(
                fanficRepository,
                new CreateFanficDtoValidator(),
                _httpContextAccessorMock.Object,
                _fanficChapterServiceMock.Object);

            const string fanficId = "01KANQK7ESG4P91Z6JRQJT4GGM";

            _fanficChapterServiceMock
                .Setup(x => x.DeleteChaptersByFanficIdAsync(fanficId))
                .ReturnsAsync(new ServiceResultDto(isSuccess: true));

            // Act

            var result = await _fanficService.DeleteFanficAsync(fanficId);

            // Assert

            Assert.True(result.IsSuccess);
        }
    }
}
