using System.Net.Http;
using Client.Utils;
using System.Text.Json;
using System.Net.Http.Json;
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
        // client = new HttpClient();
        client = http;
        client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");

    }

    public async Task<TDto> Get<TDto>(string url)
    {
        var response = await client.GetAsync(baseUrl + url); // Properly awaited

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TDto>(content, options: new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
        else
        {
            throw new Exception("Something went wrong");
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