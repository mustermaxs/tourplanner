namespace Client.Services.Search
{
    public class WordDistanceMap
    {
        public string Content { get; set; }
        public int Distance { get; set; }
        public bool IsPrefix { get; set; }

        public WordDistanceMap(string content, int distance, bool isPrefix)
        {
            Content = Content;
            Distance = distance;
            IsPrefix = isPrefix;
        }
    };
}