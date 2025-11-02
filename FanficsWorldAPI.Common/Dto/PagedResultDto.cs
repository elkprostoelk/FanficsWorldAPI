﻿namespace FanficsWorldAPI.Common.Dto
{
    public class PagedResultDto<T> where T : class
    {
        public IReadOnlyList<T> Items { get; } = [];

        public int TotalCount { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public PagedResultDto(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
