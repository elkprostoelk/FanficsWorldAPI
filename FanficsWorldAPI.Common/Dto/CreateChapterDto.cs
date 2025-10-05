namespace FanficsWorldAPI.Common.Dto
{
    public class CreateChapterDto
    {
        public string FanficId { get; set; } = string.Empty;

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string TextHtml { get; set; } = string.Empty;

        public bool IsDraft { get; set; }
    }
}
