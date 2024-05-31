using Tourplanner.Services.Search;

namespace UnitTests.Services;

[TestFixture]
[TestOf(typeof(StringSearchService))]
public class StringSearchServiceTest
{
    private ISearchService SearchService;
    
    [SetUp]
    public void Setup()
    {
        SearchService = new StringSearchService();
    }

    [Test]
    public void Returns_Correct_Matches()
    {
        List<string> searchableWords = ["asmel", "esel", "bmsel"];
        
        var matches = SearchService.GetMatches("amsel", searchableWords);

        // Assert.That(matches.All(match => searchableWords.Contains(match)));
    }

    [Test]
    public void Returns_Empty_List_When_DistanceExceeds_Threshold()
    {
        List<string> searchableWords = ["asmel", "esel", "bmsel"];
        
        var matches = SearchService.SetThreshold(0).GetMatches("threshold too large", searchableWords);

        Assert.That(matches.Count is 0);
    }
}