namespace _4LL_Monitoring.Services;

public class PeriodicApiService : BackgroundService
{
    private readonly ApiService _monitoringService;

    public PeriodicApiService(ApiService monitoringService) { _monitoringService = monitoringService; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Call your monitoring service method here
            //await _monitoringService.YourPeriodicMethodAsync();

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken) { await base.StopAsync(stoppingToken); }
}
