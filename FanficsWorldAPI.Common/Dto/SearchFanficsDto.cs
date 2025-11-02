using FanficsWorldAPI.Common.Enums;

namespace FanficsWorldAPI.Common.Dto
{
    public class SearchFanficsDto
    {
        public string? Directions { get; set; }

        public string? Ratings { get; set; }

        public string? Statuses { get; set; }

        public string? Author { get; set; }

        public string? SortBy { get; set; }

        public SortingOrder SortingOrder { get; set; } = SortingOrder.Descending;

        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
