namespace Tourplanner.Services.Search
{
    public interface ISearchService
    {
        public List<string> GetMatches(string provided, List<string> suggestions);
        public List<string> GetMatches(string userInput);
        public void SetSearchableWords(List<string> wordlist);
        public ISearchService SetThreshold(int _threshold);
    }
}