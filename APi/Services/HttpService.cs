using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http.Json;

namespace Tourplanner.Services
{
    public interface IHttpService : IDisposable
    {
        public Task<TDto> Get<TDto>(string url);
        public Task<HttpResponseMessage> Put<TDto>(TDto dto, string url);
        public Task<HttpResponseMessage> Delete(string url);
        public Task<HttpResponseMessage> Post<TDto>(TDto dto, string url);
        public HttpClient GetClient();
    }

    public class HttpService : IHttpService, IDisposable
    {
        private HttpClient client;

        public HttpService(HttpClient httpClient)
        {
            client = httpClient;
            client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("tourplanner", "1.0"));
        }

        public HttpClient GetClient() => client;

        public async Task<TDto> Get<TDto>(string url)
        {
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (typeof(TDto) == typeof(string))
                {
                    return (TDto)(object)content;
                }
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

        public void Dispose()
        {
            client.Dispose();
        }
    }
}