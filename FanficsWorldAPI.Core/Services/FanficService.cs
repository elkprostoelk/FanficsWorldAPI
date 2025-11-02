using FanficsWorldAPI.Common.Constants;
using FanficsWorldAPI.Common.Dto;
using FanficsWorldAPI.Common.Enums;
using FanficsWorldAPI.Common.Extensions;
using FanficsWorldAPI.Core.Interfaces;
using FanficsWorldAPI.DataAccess;
using FanficsWorldAPI.DataAccess.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FanficsWorldAPI.Core.Services
{
    public class FanficService : IFanficService
    {
        private readonly IRepository<Fanfic> _fanficRepository;
        private readonly IValidator<CreateFanficDto> _createFanficDtoValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FanficService(
            IRepository<Fanfic> fanficRepository,
            IValidator<CreateFanficDto> createFanficDtoValidator,
            IHttpContextAccessor httpContextAccessor)
        {
            _fanficRepository = fanficRepository;
            _createFanficDtoValidator = createFanficDtoValidator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResultDto<string>> CreateFanficAsync(CreateFanficDto createFanficDto)
        {
            await _createFanficDtoValidator.ValidateAndThrowAsync(createFanficDto);

            var userId = _httpContextAccessor.HttpContext.User.GetUserId();

            var fanfic = new Fanfic
            {
                Title = createFanficDto.Title,
                Description = createFanficDto.Description,
                Direction = createFanficDto.Direction,
                Rating = createFanficDto.Rating,
                Status = FanficStatus.InProgress,
                AuthorId = userId,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow
            };

            var created = await _fanficRepository.InsertAsync(fanfic);

            return new ServiceResultDto<string>(
                isSuccess: created,
                value: created ? fanfic.Id : null,
                errors: created ? [] : ["Failed to create a fanfic."]);
        }

        public async Task<PagedResultDto<FanficListItemDto>> SearchForFanficsAsync(
            SearchFanficsDto searchFanficsDto,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(searchFanficsDto);
            
            var fanficsQuery = _fanficRepository
                .EntitySet
                .Include(f => f.Author)
                .AsQueryable();

            fanficsQuery = ApplyFilters(fanficsQuery, searchFanficsDto);
            fanficsQuery = ApplySorting(fanficsQuery, searchFanficsDto);

            var totalCount = await fanficsQuery.CountAsync(cancellationToken);
            var items = await fanficsQuery
                .Skip((searchFanficsDto.CurrentPage - 1) * searchFanficsDto.PageSize)
                .Take(searchFanficsDto.PageSize)
                .Select(f => new FanficListItemDto
                {
                    Id = f.Id,
                    Title = f.Title,
                    Description = f.Description,
                    Direction = f.Direction,
                    Status = f.Status,
                    Rating = f.Rating,
                    CreatedDate = f.CreatedDate,
                    LastModifiedDate = f.LastModifiedDate,
                    Author = new FanficListItemAuthorDto
                    {
                        Id = f.Author.Id,
                        Name = f.Author.Name
                    }
                })
                .ToListAsync(cancellationToken);

            return new PagedResultDto<FanficListItemDto>(
                items,
                totalCount,
                searchFanficsDto.CurrentPage,
                searchFanficsDto.PageSize);
        }

        private static IQueryable<Fanfic> ApplySorting(IQueryable<Fanfic> query, SearchFanficsDto searchFanficsDto)
        {
            Expression<Func<Fanfic, dynamic>> expression = searchFanficsDto.SortBy switch
            {
                SortingFieldsConstants.Title => f => f.Title,
                SortingFieldsConstants.CreatedDate => f => f.CreatedDate,
                _ => (Fanfic f) => f.CreatedDate
            };

            return searchFanficsDto.SortingOrder == SortingOrder.Ascending
                ? query.OrderBy(expression)
                : query.OrderByDescending(expression);
        }

        private static IQueryable<Fanfic> ApplyFilters(IQueryable<Fanfic> query, SearchFanficsDto searchFanficsDto)
        {
            query = ApplyDirectionsFilter(query, searchFanficsDto.Directions);
            query = ApplyRatingsFilter(query, searchFanficsDto.Ratings);
            query = ApplyStatusesFilter(query, searchFanficsDto.Statuses);
            query = ApplyAuthorFilter(query, searchFanficsDto.Author);

            return query;
        }

        private static IQueryable<Fanfic> ApplyDirectionsFilter(IQueryable<Fanfic> query, string? directions)
        {
            if (string.IsNullOrWhiteSpace(directions))
            {
                return query;
            }

            var directionEnums = directions
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Select(d => Enum.Parse<FanficDirection>(d, true))
                    .ToList();

            if (directionEnums.Count > 0)
            {
                query = query.Where(f => directionEnums.Contains(f.Direction));
            }

            return query;
        }

        private static IQueryable<Fanfic> ApplyRatingsFilter(IQueryable<Fanfic> query, string? ratings)
        {
            if (string.IsNullOrWhiteSpace(ratings))
            {
                return query;
            }

            var ratingEnums = ratings
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Select(d => Enum.Parse<FanficRating>(d, true))
                    .ToList();

            if (ratingEnums.Count > 0)
            {
                query = query.Where(f => ratingEnums.Contains(f.Rating));
            }

            return query;
        }

        private static IQueryable<Fanfic> ApplyStatusesFilter(IQueryable<Fanfic> query, string? statuses)
        {
            if (string.IsNullOrWhiteSpace(statuses))
            {
                return query;
            }

            var ratingEnums = statuses
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Select(d => Enum.Parse<FanficStatus>(d, true))
                    .ToList();

            if (ratingEnums.Count > 0)
            {
                query = query.Where(f => ratingEnums.Contains(f.Status));
            }

            return query;
        }

        private static IQueryable<Fanfic> ApplyAuthorFilter(IQueryable<Fanfic> query, string? author)
        {
            if (string.IsNullOrWhiteSpace(author))
            {
                return query;
            }

            return query.Where(f => f.Author.Name.Contains(author));
        }
    }
}
