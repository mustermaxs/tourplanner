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
}

