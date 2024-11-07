using MudBlazor.Services;
using _4LL_Monitoring.Components;
using _4LL_Monitoring.Models;
using _4LL_Monitoring.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();                                                      // Add MudBlazor services
builder.Services.AddRazorComponents().AddInteractiveServerComponents();                 // Add services to the container.
builder.Services.AddDbContext<TycherosmonitoringContext>(options =>
                            options.UseMySql(builder.Configuration.GetConnectionString("TycherosMonitoring"),
                                    new MySqlServerVersion(new Version(11, 5, 2))));    // Add MariaDB
builder.Services.AddScoped<TycherosMonitoringService>();                                // Register the database service

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
