using MudBlazor.Services;
using _4LL_Monitoring.Components;
using _4LL_Monitoring.Models;
using _4LL_Monitoring.Services;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using DbContext = _4LL_Monitoring.Models.DbContext;

// Set the environment variable to "Production"
Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");

var builder = WebApplication.CreateBuilder(args);

// Force the application to use the Production environment
builder.Configuration["ASPNETCORE_ENVIRONMENT"] = "Production";

builder.Services.AddMudServices();                                                              // Add MudBlazor services
builder.Services.AddRazorComponents().AddInteractiveServerComponents();                         // Add services to the container.
builder.Services.AddDbContext<DbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("TycherosMonitoring"))); // Add MS SqlServer
builder.Services.AddScoped<DbService>();                                                        // Register the database service
builder.Services.AddHttpClient<GetApiResponseService>();                                       // Register HTTP client
builder.Services.AddHostedService<BackgroundCollectApiDatumService>();                         // Register the background service

// Configure Kestrel server options based on the environment
if (builder.Environment.IsProduction())
{
    builder.WebHost.ConfigureKestrel((context, options) =>
    {
        options.ListenAnyIP(5000, listenOptions =>
        {
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
        });
    });
}

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
