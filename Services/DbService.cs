using _4LL_Monitoring.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using DbContext = _4LL_Monitoring.Models.DbContext;

namespace _4LL_Monitoring.Services;

public class DbService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public DbService(IServiceScopeFactory scopeFactory) { _scopeFactory = scopeFactory; }

    #region Collected Api Data

    public async Task<List<Collectedapidatum>> GetAllCollectedData()
    {
        using var scope   = _scopeFactory.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<DbContext>();
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
            Console.WriteLine($"Number: {e.Number}, Message: {e.Message}");
            throw;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Update Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }
    }

    public async Task<List<Collectedapidatum>> GetAllCollectedData(DateTime? startDate, DateTime? endDate)
    {
        if (startDate == null) { startDate = DateTime.UtcNow; }
        if (endDate   == null) { endDate   = DateTime.UtcNow; }

        startDate = startDate.Value.Date;                       // Sets time to 00:00:00.000
        endDate   = endDate.Value.Date.AddDays(1).AddTicks(-1); // Sets time to 23:59:59.999

        using var scope   = _scopeFactory.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<DbContext>();

        try
        {
            var response = await context.Collectedapidata.Where(cd => cd.Created <= endDate && cd.Created >= startDate)
                                        .ToListAsync();
            if (response is null)
            {
                return new List<Collectedapidatum>();
            }
            return response!;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number}, Message: {e.Message}");
            throw;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Update Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }
    }

    public class AverageHourlyApiResult
    {
        public string   ApiName      { get; set; } = "";
        public DateTime Date         { get; set; }
        public int      Hour         { get; set; }
        public double   AverageValue { get; set; }
    }

    public async Task<List<AverageHourlyApiResult>> GetAverageHourlyApiResultAsync(DateTime? startDate,
        DateTime?                                                                            endDate)
    {
        if (startDate == null) { startDate = DateTime.UtcNow; }
        if (endDate   == null) { endDate   = DateTime.UtcNow; }

        startDate = startDate.Value.Date;                       // Sets time to 00:00:00.000
        endDate   = endDate.Value.Date.AddDays(1).AddTicks(-1); // Sets time to 23:59:59.999
        using var scope   = _scopeFactory.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<DbContext>();
        try
        {
            var response = await context.Collectedapidata
                                        .Where(x => x.Value.HasValue && x.Created >= startDate && x.Created <= endDate)
                                        .GroupBy(x => new {x.ApiName, x.Date.Date, x.Hour})
                                        .Select(
                                             g => new AverageHourlyApiResult
                                             {
                                                 ApiName      = g.Key.ApiName!,
                                                 Date         = g.Key.Date.Date,
                                                 Hour         = g.Key.Hour,
                                                 AverageValue = g.Average(x => x.Value!.Value),
                                             }
                                         )
                                        .ToListAsync();
            return response;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number}, Message: {e.Message}");
            throw;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Update Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }
    }

    public async Task AddColletedApiDataAsync(Collectedapidatum entity)
    {
        try
        {
            using var scope   = _scopeFactory.CreateScope();
            var       context = scope.ServiceProvider.GetRequiredService<DbContext>();
            context.Collectedapidata.Add(entity);
            await context.SaveChangesAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number}, Message: {e.Message}");
            throw;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Update Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }
    }

    public async Task UpdateEntityAsync(Collectedapidatum entity)
    {
        try
        {
            using var scope   = _scopeFactory.CreateScope();
            var       context = scope.ServiceProvider.GetRequiredService<DbContext>();
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number}, Message: {e.Message}");
            throw;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Update Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }
    }

    public async Task DeleteEntityAsync(DateTime startDate, DateTime endDate)
    {
        startDate = startDate.Date;                       // Sets time to 00:00:00.000
        endDate   = endDate.Date.AddDays(1).AddTicks(-1); // Sets time to 23:59:59.999
        using var scope   = _scopeFactory.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<DbContext>();
        var entity = await context.Collectedapidata.Where(cd => cd.Created <= endDate && cd.Created >= startDate)
                                  .ToListAsync();
        if (entity != null)
        {
            context.Collectedapidata.RemoveRange(entity);
            await context.SaveChangesAsync();
        }
    }

    #endregion

    #region Admin - User - App - Role

    #region AdminFunctions

    public async Task<List<AdminFunction>> GetAllAdminFunctions()
    {
        using var scope   = _scopeFactory.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<DbContext>();
        try
        {
            var response = await context.AdminFunctions.ToListAsync();
            if (response is null)
            {
                return new List<AdminFunction>();
            }
            return response!;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number}, Message: {e.Message}");
            throw;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Update Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }
    }

    public async Task AddAdminFunctions(string adminFunction)
    {
        using var scope   = _scopeFactory.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<DbContext>();
        try
        {
            if (adminFunction == null)
            {
                throw new ArgumentException("AdminFunctions collection is null or empty.", nameof(adminFunction));
            }

            var rec = new AdminFunction
            {
                FunctionID = adminFunction,
            };
            await context.AdminFunctions.AddAsync(rec);
            await context.SaveChangesAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number}, Message: {e.Message}");
            throw;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Update Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }
    }

    public async Task DeleteAdminFunction(string adminFunctionId)
    {
        using var scope   = _scopeFactory.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<DbContext>();
        try
        {
            if (string.IsNullOrWhiteSpace(adminFunctionId))
            {
                throw new ArgumentException("AdminFunction ID is null or empty.", nameof(adminFunctionId));
            }

            var adminFunction = await context.AdminFunctions.FindAsync(adminFunctionId);
            if (adminFunction == null)
            {
                throw new InvalidOperationException($"AdminFunction with ID '{adminFunctionId}' not found.");
            }

            context.AdminFunctions.Remove(adminFunction);
            await context.SaveChangesAsync();
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number}, Message: {e.Message}");
            throw;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Update Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }
    }

    #endregion

    #region AdminRoles

    public async Task<List<AdminRole>> GetAllAdminRoles()
    {
        using var scope   = _scopeFactory.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<DbContext>();
        try
        {
            var response = await context.AdminRoles.ToListAsync();
            if (response is null)
            {
                return new List<AdminRole>();
            }
            return response!;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number}, Message: {e.Message}");
            throw;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Update Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }
    }

    #endregion

    #region AdminUsers

    public async Task<List<AdminUser>> GetAllAdminUsers()
    {
        using var scope   = _scopeFactory.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<DbContext>();
        try
        {
            var response = await context.AdminUsers.ToListAsync();
            if (response is null)
            {
                return new List<AdminUser>();
            }
            return response!;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number}, Message: {e.Message}");
            throw;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Update Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }
    }

    #endregion

    #region AdminApps

    public async Task<List<AdminApp>> GetAllAdminApps()
    {
        using var scope   = _scopeFactory.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<DbContext>();
        try
        {
            var response = await context.AdminApps.ToListAsync();
            if (response is null)
            {
                return new List<AdminApp>();
            }
            return response!;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number}, Message: {e.Message}");
            throw;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Update Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }
    }

    #endregion

    #region AdminUserAppStates

    public async Task<List<AdminUserAppState>> GetAllAdminUserAppStates()
    {
        using var scope   = _scopeFactory.CreateScope();
        var       context = scope.ServiceProvider.GetRequiredService<DbContext>();
        try
        {
            var response = await context.AdminUserAppStates.ToListAsync();
            if (response is null)
            {
                return new List<AdminUserAppState>();
            }
            return response!;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Number: {e.Number}, Message: {e.Message}");
            throw;
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Update Exception: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
            throw;
        }
    }

    #endregion

    #endregion
}
