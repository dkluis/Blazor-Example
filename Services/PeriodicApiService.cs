namespace _4LL_Monitoring.Services;

public class PeriodicApiService : BackgroundService
{
    private readonly ApiService _apiService;

    public PeriodicApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var apiUrl = "https://uptimeapi.myapiserve.com/UptimeApi/CheckOrders";
            var (status, result) = await _apiService.GetApiResultAsync(apiUrl);

            apiUrl = "https://uptimeapi.myapiserve.com/UptimeApi/CheckRegistrations";
            (status,result) = await _apiService.GetApiResultAsync(apiUrl);

            apiUrl = "https://uptimeapi.myapiserve.com/UptimeApi/CheckLogins";
            (status, result) = await _apiService.GetApiResultAsync(apiUrl);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken) { await base.StopAsync(stoppingToken); }
}
