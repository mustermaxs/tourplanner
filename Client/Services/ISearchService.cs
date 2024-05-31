namespace Client.Services.Search
{
    public interface ISearchService
    {
        public List<string> GetMatches(string provided, List<string> suggestions);
        public void SetSearchableWords(List<string> wordlist);
    }
}