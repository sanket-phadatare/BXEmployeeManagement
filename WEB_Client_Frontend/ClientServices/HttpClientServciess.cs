using System.Net;
using System.Net.Http;

namespace WEB_Client_Frontend.ClientServices
{
    public class HttpClientServciess : IHttpClientServices
    {
        private readonly HttpClient _client;


        public HttpClientServciess(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("HttpClientServciess");
        }

        public async Task<T> DeleteAsync<T>(string requestUri)
        {
            var response = await _client.DeleteAsync(requestUri);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T> DeleteWithBodyAsync<T>(string requestUri, object content)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_client.BaseAddress + requestUri),
                Content = JsonContent.Create(content)  // uses System.Net.Http.Json
            };

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }


        public async Task<T> GetAsync<T>(string requestUri)
        {
            var response = await _client.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T> PostAsync<T>(string requestUri, object content)
        {
            var response = await _client.PostAsJsonAsync(requestUri, content);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> PutAsync<T>(string requestUri, object content)
        {
            var response = await _client.PutAsJsonAsync(requestUri, content);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default; // return null
                }

                return await response.Content.ReadFromJsonAsync<T>();
            }

            return default;
        }

    }
}
