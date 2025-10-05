using FanficsWorldAPI.Common.Enums;
using NUlid;

namespace FanficsWorldAPI.DataAccess.Entities
{
    public class Fanfic
    {
        public string Id { get; set; } = Ulid.NewUlid().ToString();

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public FanficDirection Direction { get; set; }

        public FanficStatus Status { get; set; }

        public FanficRating Rating { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string AuthorId { get; set; } = string.Empty;

        public User Author { get; set; } = null!;

        public List<FanficChapter> Chapters { get; set; } = [];
    }
}
