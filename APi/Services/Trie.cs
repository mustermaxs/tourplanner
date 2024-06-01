namespace Tourplanner.Services.Search
{
    public class Trie
    {
        private TrieNode root;
        private bool ignoreCase;

        public Trie(bool caseInsensitive)
        {
            root = new TrieNode();
            ignoreCase = caseInsensitive;
        }

        public void Init(List<string> wordList)
        {
            foreach (var word in wordList)
            {
                Insert(word);
            }
        }

        private int CharToIndex(char character)
        {
            return ignoreCase
                ? char.ToLower(character) - 'a'
                : character - 'a';
        }

        public bool Insert(string word)
        {
            try
            {
                if (WordExists(word))
                    return true;

                TrieNode node = root;

                foreach (var character in word)
                {
                    int index = CharToIndex(character);

                    if (node.Children.ElementAtOrDefault(index) is null)
                    {
                        node.Children[index] = new TrieNode();
                    }

                    node = node.Children.ElementAt(index);
                }

                node.IsTerminal = true;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool WordExists(string word)
        {
            TrieNode node = root;

            foreach (var character in word)
            {
                int index = CharToIndex(character);

                if (node.Children.ElementAtOrDefault(index) is null)
                {
                    return false;
                }

                node = node.Children.ElementAt(index);
            }

            return node.IsTerminal;
        }

        public bool PrefixExists(string prefix)
        {
            TrieNode node = root;

            foreach (var character in prefix)
            {
                int index = CharToIndex(character);

                if (node.Children.ElementAtOrDefault(index) is null)
                {
                    return false;
                }

                node = node.Children.ElementAt(index);
            }

            return true;
        }
    }
}