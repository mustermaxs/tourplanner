namespace Client.Services.Search
{
    public class TrieNode
    {
        public TrieNode[] Children { get; set; } = new TrieNode[130];
        public bool IsTerminal { get; set; }

        public TrieNode(bool isTerminal = false)
        {
            IsTerminal = isTerminal;
        }
    } 
}
