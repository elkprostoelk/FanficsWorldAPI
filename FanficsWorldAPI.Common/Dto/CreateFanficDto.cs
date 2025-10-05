using FanficsWorldAPI.Common.Enums;

namespace FanficsWorldAPI.Common.Dto
{
    public class CreateFanficDto
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public FanficDirection Direction { get; set; }

        public FanficRating Rating { get; set; }
    }
}
