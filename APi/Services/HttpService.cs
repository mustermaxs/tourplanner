using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http.Json;
using Api.Services.Logging;
using Tourplanner.Exceptions;
using LoggerFactory = Api.Services.Logging.LoggerFactory;

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
        protected ILoggerWrapper Logger = LoggerFactory.GetLogger();


        public HttpService(HttpClient httpClient)
        {
            client = httpClient;
            client.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("tourplanner", "1.0"));
        }

        public HttpClient GetClient() => client;

        public async Task<TDto> Get<TDto>(string url)
        {
            try
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
                    Logger.Warn($"HttpService: Failed to get data. Url: {url}");
                    throw new DataAccessLayerException("HttpService: Failed to get data");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Logger.Error($"HttpService: Failed to get data. Url: {url}. Exception: {e.Message}");
                throw new DataAccessLayerException("HttpService: Failed to get data", e);
            }
        }


        public async Task<HttpResponseMessage> Post<TDto>(TDto dto, string url)
        {
            try
            {
                return await client.PostAsJsonAsync(url, dto);
            }
            catch (Exception e)
            {
                Logger.Error($"HttpService: [POST] Failed. {url}. Exception: {e.Message}");
                throw new DataAccessLayerException("HttpService: [POST] Failed", e);
            }
        }

        public async Task<HttpResponseMessage> Put<TDto>(TDto dto, string url)
        {
            try
            {
                return await client.PutAsJsonAsync(url, dto);
            }
            catch (Exception e)
            {
                Logger.Error($"HttpService: [PUT] Failed. {url}. Exception: {e.Message}");
                throw new DataAccessLayerException("HttpService: [PUT] Failed", e);
            }
        }

        public async Task<HttpResponseMessage> Delete(string url)
        {
            try
            {
                return await client.DeleteAsync(url);
            }
            catch (Exception e)
            {
                Logger.Error($"HttpService: [DELETE] Failed. {url}. Exception: {e.Message}");
                throw new DataAccessLayerException("HttpService: [DELETE] Failed", e);
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}