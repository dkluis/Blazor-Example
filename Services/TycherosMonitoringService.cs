using _4LL_Monitoring.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace _4LL_Monitoring.Services;

public class TycherosMonitoringService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public TycherosMonitoringService(IServiceScopeFactory scopeFactory) { _scopeFactory = scopeFactory; }

    #region CollectedApiDatum

    public async Task<List<Collectedapidatum>> GetAllEntitiesAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TycherosmonitoringContext>();
        try
        {
            var response = await context.Collectedapidata.ToListAsync();
            if (response is null)
            {
                return new List<Collectedapidatum>();
            }
            return response!;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number} , Message: {e.Message}");
            throw;
        }
    }

    public class AverageHourlyApiResult
    {
        public string ApiName { get; set; }
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public double AverageValue { get; set; }
    }

    public async Task<List<AverageHourlyApiResult>> GetAverageHourlyApiResultAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TycherosmonitoringContext>();
        try
        {
            var response = await context.Collectedapidata.Where(x => x.Value.HasValue)
                                        .GroupBy(x => new {x.ApiName, x.Date.Date, x.Hour})
                                        .Select(
                                                 g => new AverageHourlyApiResult
                                                 {
                                                     ApiName = g.Key.ApiName!,
                                                     Date = g.Key.Date.Date,
                                                     Hour = g.Key.Hour,
                                                     AverageValue = g.Average(x => x.Value!.Value),
                                                 }
                                         )
                                        .ToListAsync();
            return response;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number} , Message: {e.Message}");
            throw;
        }
    }

    public async Task<Collectedapidatum?> GetEntityByIdAsync(int id)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TycherosmonitoringContext>();
        return await context.Collectedapidata.FindAsync(id);
    }

    public async Task AddColletedApiDataAsync(Collectedapidatum entity)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TycherosmonitoringContext>();
        context.Collectedapidata.Add(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateEntityAsync(Collectedapidatum entity)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TycherosmonitoringContext>();
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteEntityAsync(int id)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TycherosmonitoringContext>();
        var entity = await context.Collectedapidata.FindAsync(id);
        if (entity != null)
        {
            context.Collectedapidata.Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    #endregion
}
