using Client.Models;

namespace Client.Dto
{
    public class SearchResultsDto<TEntity>
    {
        public readonly string SearchTerm;
        public readonly string Category;
        public readonly IEnumerable<TEntity> Matches;

        public SearchResultsDto(string searchTerm, string category, IEnumerable<TEntity> matches)
        {
            SearchTerm = searchTerm;
            Matches = matches;
            Category = category;
        }
    }

    public class GlobalSearchResultsDto
    {
        public string SearchTerm { get; set; }
        public IEnumerable<Tour> Tours { get; set; }
        public IEnumerable<TourLog> TourLogs { get; set; }
        public bool Successful { get; set; }

        public GlobalSearchResultsDto()
        {
            
        }

        public GlobalSearchResultsDto(string searchTerm, IEnumerable<Tour> tours, IEnumerable<TourLog> tourLogs,
            bool successful)
        {
            SearchTerm = searchTerm;
            Tours = tours;
            TourLogs = tourLogs;
            Successful = successful;
        }
    }
}