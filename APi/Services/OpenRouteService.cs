using System.Text.Json.Nodes;

namespace Tourplanner.Services
{
    public interface IOpenRouteService
    {
        public Task<JsonObject> GetAutoCompleteSuggestions(string query);

    }
    public class OpenRouteService : IOpenRouteService
    {
        private readonly IHttpService _httpService;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private string _autoCompleteUri { get; set; }
        private string _apiKey { get; set; }

        public OpenRouteService(IHttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _configuration = configuration;
            _apiKey = _configuration["OpenRouteService:ApiKey"];
            _autoCompleteUri = _configuration["OpenRouteService:Uri:AutoComplete"];
            _baseUrl = _configuration["OpenRouteService:BaseUrl"];
        }
        
        public async Task<JsonObject> GetAutoCompleteSuggestions(string query)
        {
            return  await _httpService.Get<JsonObject>($"{_baseUrl}{_autoCompleteUri}api_key={_apiKey}&text={query}");
        }
    }
}