using MudBlazor.Services;
using _4LL_Monitoring.Components;
using _4LL_Monitoring.Models;
using _4LL_Monitoring.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();                                                              // Add MudBlazor services
builder.Services.AddRazorComponents().AddInteractiveServerComponents();                         // Add services to the container.
builder.Services.AddDbContext<TycherosmonitoringContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("TycherosMonitoring"))); // Add MS SqlServer
builder.Services.AddScoped<TycherosMonitoringService>();                                        // Register the database service
builder.Services.AddHttpClient<ApiService>();                                                   // Register HTTP client
builder.Services.AddHostedService<PeriodicApiService>();                                        // Register the background service

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
