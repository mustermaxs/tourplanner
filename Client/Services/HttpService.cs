using System.Net.Http;
using Client.Utils;
using System.Text.Json;
using System.Net.Http.Json;
using Client.Exceptions;
using Client.Pages;

public interface IHttpService
{
    public Task<TDto> Get<TDto>(string url);
    public Task<HttpResponseMessage> Put<TDto>(TDto dto, string url);
    public Task<HttpResponseMessage> Delete(string url);
    public Task<HttpResponseMessage> Post<TDto>(TDto dto, string url);
}
public class HttpService : IHttpService
{
    private String baseUrl;
    private HttpClient client;
    public HttpService(HttpClient http)
    {
        baseUrl = StaticResService.GetApiBaseUrl;
        client = http;
        client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");

    }

    public async Task<TDto> Get<TDto>(string url)
    {
        var response = await client.GetAsync(baseUrl + url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                JsonElement jsonElement = JsonSerializer.Deserialize<JsonElement>(content);
                return JsonSerializer.Deserialize<TDto>(content, options: new JsonSerializerOptions(JsonSerializerDefaults.Web));
            }
            catch (JsonException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        else
        {
            throw new ServiceLayerException("HttpService: Failed to get data");
        }
    }


    public async Task<HttpResponseMessage> Post<TDto>(TDto dto, string url)
    {
        return await client.PostAsJsonAsync(baseUrl + url, dto);
    }

    public async Task<HttpResponseMessage> Put<TDto>(TDto dto, string url)
    {
        return await client.PutAsJsonAsync(baseUrl + url, dto);
    }

    public async Task<HttpResponseMessage> Delete(string url)
    {
        return await client.DeleteAsync(baseUrl + url);
    }
}