namespace Tourplanner.Services.Search
{
    public class StringSearchService : ISearchService
    {
        private Trie prefixTree;
        private List<string> searchableWords;
        private bool completedSetup;
        private FuzzyMatcher fuzzyMatcher;
        private int threshold;

        public StringSearchService()
        {
            fuzzyMatcher = new FuzzyMatcher();
            prefixTree = new Trie(true);
            searchableWords = new List<string>();
            completedSetup = false;
            threshold = Int32.MaxValue;
        }

        public void SetSearchableWords(List<string> words)
        {
            searchableWords = words.Select(word => word.ToLower()).ToList();
            prefixTree.Init(words);
            completedSetup = true;
        }

        private int CbSortByDistanceAsc(WordDistanceMap currentWord, WordDistanceMap nextWord)
        {
            return currentWord.Distance <= nextWord.Distance ? -1 : 1;
        }

        private int CbSortByPrefixAsc(WordDistanceMap currentWord, WordDistanceMap nextWord)
        {
            if (currentWord.IsPrefix && !nextWord.IsPrefix)
                return -1;
            if (!currentWord.IsPrefix && nextWord.IsPrefix)
                return 1;
            if (!currentWord.IsPrefix && !nextWord.IsPrefix)
                return 0;
            if (currentWord.IsPrefix && currentWord.Distance <= nextWord.Distance)
                return -1;
            return 1;
        }

        private List<WordDistanceMap> SortSuggestions(List<WordDistanceMap> distanceMappings)
        {
            var sorted = distanceMappings.OrderBy(word => word.Distance).ToList();
            sorted.Sort(CbSortByPrefixAsc);
            return sorted;
        }

        /// <summary>
        /// Boundary for Levenstein distance where matches get dropped if their distance exceeds the threshold.
        /// Higher threshold => more forgiving => more search results
        /// Lower threshold => less forgiving => fewer search results 
        /// </summary>
        /// <param name="_threshold"></param>
        /// <returns>Instance itself (for method chaining)</returns>
        public ISearchService SetThreshold(int _threshold)
        {
            threshold = _threshold;
            return this;
        }
        
        /// <summary>
        /// Gets possible matches for a provided search term.
        /// Use this implementation if you don't plan to reuse the wordlist.
        /// </summary>
        /// <param name="userInput">Search term.</param>
        /// <param name="wordList">List of searchable words.</param>
        /// <returns></returns>
        public List<string> GetMatches(string userInput, List<string> wordList)
        {
            SetSearchableWords(wordList);
            wordList = wordList.Select(word => word.ToLower()).ToList();
            var stringDistanceMappings = CreateDistanceMappingsForWordList(wordList, userInput);
            stringDistanceMappings = SortSuggestions(stringDistanceMappings);
            var matches = stringDistanceMappings
                .Where(mapping => mapping.Distance <= threshold)
                .Select(word => word.Content).ToList();
            
            Reset();

            return matches;
        }
        
        /// <summary>
        /// Gets possible matches for a provided search term.
        /// Use this implementation if you already defined a list of searchable words using
        /// the method SetSearchableWords.
        /// </summary>
        /// <param name="userInput">Search term.</param>
        /// <returns>List of possible matches.</returns>
        /// <exception cref="Exception">Throws exception if setup is incomplete (e.g. no list of searchable
        /// words provided.</exception>
        public List<string> GetMatches(string userInput)
        {
            if (!completedSetup)
                throw new Exception("Setup incomplete!");
            
            var matches = searchableWords.Select(word => word.ToLower()).ToList();
            var stringDistanceMappings = CreateDistanceMappingsForWordList(matches, userInput);
            stringDistanceMappings = SortSuggestions(stringDistanceMappings);

            return stringDistanceMappings.Select(word => word.Content).ToList();
        }

        private string LimitWordLength(string word, int length)
        {
            return word.Substring(0, Math.Min(length, word.Length));
        }

        private int GetShorterDistance(string userInput, string word)
        {
            int userInputLength = userInput.Length;
            string trimmedWord = word.Substring(0, Math.Min(userInputLength, word.Length));
            int trimmedWordDistance = fuzzyMatcher.GetEditDistance(userInput, trimmedWord);
            int untrimmedWordDistance = fuzzyMatcher.GetEditDistance(userInput, word);

            return Math.Min(trimmedWordDistance, untrimmedWordDistance);
        }

        private List<WordDistanceMap> CreateDistanceMappingsForWordList(List<string> wordList, string userInput)
        {
            bool existsAsPrefix = prefixTree.PrefixExists(userInput);

            return wordList.Select(word =>
            {
                bool isPrefix = existsAsPrefix && word.StartsWith(userInput);
                int distance = GetShorterDistance(userInput, word);
                return new WordDistanceMap(word, distance, isPrefix);
            }).ToList();
        }

        private void Reset()
        {
            completedSetup = false;
            searchableWords = new List<string>();
            prefixTree = new Trie(true);
            threshold = Int32.MaxValue;
        }
    }
}