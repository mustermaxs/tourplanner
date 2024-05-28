namespace Tourplanner.DTOs
{
    
    public class SearchResult<T
    
    
    public class SearchResultsDto
    {
        public SearchResultsDto(
            string SearchTerm,
            IEnumerable<TourDto> Tours,
            IEnumerable<TourLogDto> TourLogs);
    }
}

