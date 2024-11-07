using _4LL_Monitoring.Models;
using Microsoft.EntityFrameworkCore;

namespace _4LL_Monitoring.Services;

public class TycherosMonitoringService
{
    private readonly TycherosmonitoringContext _context;

    public TycherosMonitoringService(TycherosmonitoringContext context)
    {
        _context = context;
    }

    #region CollectedApiDatum

    public async Task<List<Collectedapidatum?>> GetAllEntitiesAsync()
    {
        return await _context.Collectedapidata.ToListAsync();
    }

    public async Task<Collectedapidatum?> GetEntityByIdAsync(int id)
    {
        return await _context.Collectedapidata.FindAsync(id);
    }

    public async Task AddEntityAsync(Collectedapidatum? entity)
    {
        _context.Collectedapidata.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEntityAsync(Collectedapidatum entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEntityAsync(int id)
    {
        var entity = await _context.Collectedapidata.FindAsync(id);
        if (entity != null)
        {
            _context.Collectedapidata.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    #endregion
}
