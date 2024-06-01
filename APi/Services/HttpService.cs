using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;

namespace Tourplanner.Services
{
    public interface IHttpService
    {
        public Task<TDto> Get<TDto>(string url);
        public Task<HttpResponseMessage> Put<TDto>(TDto dto, string url);
        public Task<HttpResponseMessage> Delete(string url);
        public Task<HttpResponseMessage> Post<TDto>(TDto dto, string url);
    }

    public class HttpService : IHttpService
    {
        private HttpClient client;

        public HttpService(HttpClient httpClient)
        {
            client = httpClient;
            client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
        }

        public async Task<TDto> Get<TDto>(string url)
        {
            var response = await client.GetAsync(url); // Properly awaited

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TDto>(content,
                    options: new JsonSerializerOptions(JsonSerializerDefaults.Web));
            }
            else
            {
                throw new Exception("Something went wrong");
            }
        }


        public async Task<HttpResponseMessage> Post<TDto>(TDto dto, string url)
        {
            return await client.PostAsJsonAsync(url, dto);
        }

        public async Task<HttpResponseMessage> Put<TDto>(TDto dto, string url)
        {
            return await client.PutAsJsonAsync(url, dto);
        }

        public async Task<HttpResponseMessage> Delete(string url)
        {
            return await client.DeleteAsync(url);
        }
    }
}