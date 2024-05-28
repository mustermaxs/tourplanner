using Tourplanner.Infrastructure;
using Tourplanner.Services.Search;
using Tourplanner.DTOs;
using Tourplanner.Repositories;

namespace Tourplanner.Entities.Tours
{
    public record GetSearchResultsQuery
    (
        string SearchTerm,
        string Category,
        int Threshold
    ) : IRequest;

    public class GetSearchResultsQueryHandler(
        TourContext ctx,
        ITourRepository tourRepository,
        ITourLogRepository tourLogRepository,
        ISearchService searchService) : RequestHandler<GetSearchResultsQuery, SearchResultsDto>(ctx)
    {
        public override async Task<SearchResultsDto> Handle(GetSearchResultsQuery query)
        {
            var tours = await tourRepository.GetAll();
            var tourlogs = await tourLogRepository.GetAll();

            var searchables = new List<string>();

            foreach (var tour in tours)
            {
                searchables.AddRange(
                    new List<string> { tour.Name, tour.From, tour.To, tour.Id.ToString() });
            }
            foreach (var log in tourlogs)
            {
                searchables.AddRange(
                    new List<string> {log. });
            }
            var searchables = tours.Select(tour =>
            {
                var words = new List<string> { tour.Name, tour.From, tour.To, tour.Id.ToString() };
                
            });
        }
    }
}