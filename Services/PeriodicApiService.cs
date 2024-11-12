using _4LL_Monitoring.Models;

namespace _4LL_Monitoring.Services;

public class PeriodicApiService : BackgroundService
{
    private readonly ApiService _apiService;
    //private readonly TycherosmonitoringContext _dbContext;

    public PeriodicApiService(ApiService apiService)
    {
        _apiService = apiService;
        //_dbContext = dbContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var apiUrl = "https://uptimeapi.myapiserve.com/UptimeApi/CheckOrders";
            var (status, result) = await _apiService.GetApiResultAsync(apiUrl);
            var rec = new Collectedapidatum
            {
                HttpStatusCode = (int)status,  ApiName = "Test", Type = "Threshold",
                Threshold = 10, Value = 11, Status = "Ok", Note = null,
                JsonResponse = result, ErrorDetails = null,
            };
            //_dbContext.Collectedapidata.Add(rec);

            apiUrl = "https://uptimeapi.myapiserve.com/UptimeApi/CheckRegistrations";
            (status,result) = await _apiService.GetApiResultAsync(apiUrl);

            apiUrl = "https://uptimeapi.myapiserve.com/UptimeApi/CheckLogins";
            (status, result) = await _apiService.GetApiResultAsync(apiUrl);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken) { await base.StopAsync(stoppingToken); }
}
