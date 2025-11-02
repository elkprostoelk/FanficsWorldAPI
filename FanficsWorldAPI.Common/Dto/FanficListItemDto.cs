using FanficsWorldAPI.Common.Enums;

namespace FanficsWorldAPI.Common.Dto
{
    public class FanficListItemDto
    {
        public string Id { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public FanficDirection Direction { get; set; }

        public FanficStatus Status { get; set; }

        public FanficRating Rating { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public FanficListItemAuthorDto Author { get; set; } = new();
    }
}
