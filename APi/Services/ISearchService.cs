namespace Tourplanner.Services.Search
{
    public interface ISearchService
    {
        public List<WordDistanceMap> GetMatches(string provided, List<string> searchables);
        public List<WordDistanceMap> GetMatches(string userInput);
        public void SetSearchableWords(List<string> wordlist);
        
        public ISearchService SetThreshold(int _threshold);
    }
}