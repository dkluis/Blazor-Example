using System.Diagnostics;
using System.Net;

namespace _4LL_Monitoring.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(HttpStatusCode, long, string)> GetApiResultAsync(string apiUrl)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                stopwatch.Stop();
                var elapsed = stopwatch.ElapsedMilliseconds;
                return (response.StatusCode, elapsed, result ?? "API result is null");
            }
            catch (Exception ex)
            {
                return (HttpStatusCode.InternalServerError, 0, "An error occurred while fetching the API result");
            }
        }
    }
}
