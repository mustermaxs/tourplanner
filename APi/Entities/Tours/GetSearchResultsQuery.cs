using Microsoft.EntityFrameworkCore;
using Tourplanner.Infrastructure;
using Tourplanner.Services.Search;
using Tourplanner.DTOs;
using Tourplanner.Entities.TourLogs;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.Tours
{
    public record GetSearchResultsQuery(
        string SearchTerm
    ) : IRequest;

    public class GetSearchResultsQueryHandler(
        TourContext ctx,
        ITourRepository tourRepository,
        ITourLogRepository tourLogRepository,
        ISearchService searchService)
        : RequestHandler<GetSearchResultsQuery, IEnumerable<SearchResultsDto<object>>>(ctx)
    {
        public override async Task<IEnumerable<SearchResultsDto<object>>> Handle(GetSearchResultsQuery query)
        {
            var searchTerm = query.SearchTerm ?? string.Empty;
            var tours = await tourRepository.GetAll();
            var tourlogs = await tourLogRepository.GetAll();

            var searchables = new List<string>();
            var results = new List<SearchResultsDto<object>>();

            var tourMatches = await dbContext.Tours.Where(tour =>
                    EF.Functions.ILike(tour.Name, $"%{searchTerm}%") ||
                    EF.Functions.ILike(tour.Description, $"%{searchTerm}%") ||
                    EF.Functions.ILike(tour.From, $"%{searchTerm}%") ||
                    EF.Functions.ILike(tour.To, $"%{searchTerm}%"))
                .Select(match => match.ToTourDto())
                .ToListAsync();

            var tourSearchResults = new SearchResultsDto<object>(query.SearchTerm, "Tours", tourMatches);

            var tourLogMatches = await dbContext.TourLogs
                .Where(log =>
                    EF.Functions.ILike(log.Comment, $"%{searchTerm}%"))
                .Select(log => log.ToTourLogDto())
                .ToListAsync();

            var logSearchResults = new SearchResultsDto<object>(query.SearchTerm, "TourLogs", tourLogMatches);

            results.Add(tourSearchResults);
            results.Add(logSearchResults);

            return results;
        }
    }
}