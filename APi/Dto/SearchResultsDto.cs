namespace Tourplanner.DTOs
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
        public readonly string SearchTerm;
        public readonly IEnumerable<TourDto> Tours;
        public readonly IEnumerable<TourLogDto> TourLogs;
        public readonly bool Successful;

        public GlobalSearchResultsDto(string searchTerm, IEnumerable<TourDto> tours, IEnumerable<TourLogDto> tourLogs, bool successful)
        {
            SearchTerm = searchTerm;
            Tours = tours;
            TourLogs = tourLogs;
            Successful = successful;
        }
    }
}

