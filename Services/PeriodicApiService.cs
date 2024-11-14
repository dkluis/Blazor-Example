using System.Text.Json;
using System.Text.Json.Serialization;
using _4LL_Monitoring.Models;

namespace _4LL_Monitoring.Services;

public class PeriodicApiService : BackgroundService
{
    private readonly ApiService _apiService;
    private readonly IServiceProvider _serviceProvider;

    public PeriodicApiService(ApiService apiService, IServiceProvider serviceProvider)
    {
        _apiService = apiService;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var apiUrl = "https://uptimeapi.myapiserve.com/UptimeApi/CheckOrders";
            var (status, elapsed, result) = await _apiService.GetApiResultAsync(apiUrl);

            var ordersDto = ConvertJsonToData<CheckOrdersDto>(result);
            var orderRec = new Collectedapidatum
            {
                HttpStatusCode = (int) status,
                Created = DateTime.UtcNow,
                ApiName = ordersDto.ApiName,
                Type = ordersDto.Type,
                Threshold = ordersDto.Minimum,
                Value = ordersDto.Count,
                Status = ordersDto.Status,
                Note = "Timespan 1 hour",
                JsonResponse = result,
                ErrorDetails = null,
                ElapsedMilliseconds = elapsed,
            };
            using var scope = _serviceProvider.CreateScope();
            var dbService = scope.ServiceProvider.GetRequiredService<TycherosMonitoringService>();
            await dbService.AddColletedApiDataAsync(orderRec);

            apiUrl = "https://uptimeapi.myapiserve.com/UptimeApi/CheckRegistrations";
            (status, elapsed, result) = await _apiService.GetApiResultAsync(apiUrl);

            var registrationDto = ConvertJsonToData<CheckRegistrationsDto>(result);
            var regRec = new Collectedapidatum
            {
                HttpStatusCode = (int) status,
                Created = DateTime.UtcNow,
                ApiName = registrationDto.ApiName,
                Type = registrationDto.Type,
                Threshold = registrationDto.Minimum,
                Value = registrationDto.Count,
                Status = registrationDto.Status,
                Note = "Timespan 1 hour",
                JsonResponse = result,
                ErrorDetails = null,
                ElapsedMilliseconds = elapsed,
            };
            await dbService.AddColletedApiDataAsync(regRec);

            apiUrl = "https://uptimeapi.myapiserve.com/UptimeApi/CheckLogins";
            (status, elapsed, result) = await _apiService.GetApiResultAsync(apiUrl);

            var loginDto = ConvertJsonToData<CheckLoginsDto>(result);
            var loginRec = new Collectedapidatum
            {
                HttpStatusCode = (int) status,
                Created = DateTime.UtcNow,
                ApiName = loginDto.ApiName,
                Type = loginDto.Type,
                Threshold = loginDto.Minimum,
                Value = loginDto.Count,
                Status = loginDto.Status,
                Note = "Timespan 1 hour",
                JsonResponse = result,
                ErrorDetails = null,
                ElapsedMilliseconds = elapsed,
            };
            await dbService.AddColletedApiDataAsync(loginRec);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken) { await base.StopAsync(stoppingToken); }

    public class CheckOrdersDto
    {
        public string ApiName { get; set; } = "CheckOrders";
        public string Type { get; set; } = "Threshold";
        [JsonPropertyName("orderCount")]
        public int Count { get; set; }
        [JsonPropertyName("minimumOrders")]
        public int Minimum { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    public class CheckLoginsDto
    {
        public string ApiName { get; set; } = "CheckLogins";
        public string Type { get; set; } = "Threshold";
        [JsonPropertyName("loginCount")]
        public int Count { get; set; }
        [JsonPropertyName("minimumLogins")]
        public int Minimum { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    public class CheckRegistrationsDto
    {
        public string ApiName { get; set; } = "CheckRegistrations";
        public string Type { get; set; } = "Threshold";
        [JsonPropertyName("registrationsCount")]
        public int Count { get; set; }
        [JsonPropertyName("minimumRegistrations")]
        public int Minimum { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

        public static T ConvertJsonToData<T>(string json) where T : class
        {
            var apiDataDto = JsonSerializer.Deserialize<T>(json);
            if (apiDataDto == null)
            {
                throw new ArgumentException("Invalid JSON data");
            }
            return apiDataDto;
        }
}
