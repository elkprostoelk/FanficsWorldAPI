using NUlid;

namespace FanficsWorldAPI.DataAccess.Entities
{
    public class FanficChapter
    {
        public string Id { get; set; } = Ulid.NewUlid().ToString();

        public string? Title { get; set; }

        public int Number { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public bool IsDraft { get; set; }

        public string TextHtml { get; set; } = string.Empty;

        public string FanficId { get; set; } = string.Empty;

        public Fanfic Fanfic { get; set; } = null!;
    }
}
