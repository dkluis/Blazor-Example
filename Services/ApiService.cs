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

        public async Task<(HttpStatusCode, string)> GetApiResultAsync(string apiUrl)
        {
            try
            {
                // Create the HTTP request
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                // Send the request
                var response = await _httpClient.SendAsync(request);
                // Ensure the response is successful
                response.EnsureSuccessStatusCode();
                // Read and return the response content
                var result = await response.Content.ReadAsStringAsync();
                // Return the response content or a default message if null
                return (response.StatusCode, result ?? "API result is null");
            }
            catch (Exception ex)
            {
                return (HttpStatusCode.InternalServerError, "An error occurred while fetching the API result");
            }
        }
    }
}
