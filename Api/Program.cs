using Serilog;
using Api.W3;
using Api.AI;
using Api.WebScraping;
using Api.Accessibility;
using Api.Accessibility.Services;
using Api.Accessibility.Services.Rules;
using Api.Configuration;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add environment file configuration
builder.Configuration.AddEnvironmentFile();

// Validate and load settings
var settings = builder.Configuration.GetValidatedSettings();
builder.Services.AddSingleton(settings);

// Configure URLs
builder.WebHost.UseUrls("http://0.0.0.0:5001");

// Configure forwarded headers
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Trust the local Nginx proxy
    options.KnownProxies.Add(System.Net.IPAddress.Parse("127.0.0.1"));
});

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        Path.Combine("logs", "log-.txt"),
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true)
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    builder.Host.UseSerilog();

    // Add services to the container
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "API",
            Version = "v1"
        });
    });
    builder.Services.AddHealthChecks();

    // Configure CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    // Add W3 HTML validation service
    builder.Services.AddScoped<IW3Service, W3Service>();

    // Add Web Scraping service
    builder.Services.AddScoped<IWebScraperService, PlaywrightScraperService>();

    // Add Accessibility services
    builder.Services.AddScoped<IRuleEngine, RuleEngine>();
    builder.Services.AddScoped<IAccessibilityAnalyzer, AccessibilityAnalyzer>();
    builder.Services.AddScoped<ISection1Service, Section1Service>();

    // Add SQLite
    var dbPath = builder.Environment.IsProduction() 
        ? Path.Combine(Environment.GetEnvironmentVariable("DEPLOY_PATH") ?? "", settings.Database.SqlitePath)
        : settings.Database.SqlitePath;

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite($"Data Source={dbPath}"));

    // Add HttpClient
    builder.Services.AddHttpClient();

    // Add AI services
    builder.Services.AddScoped<OpenAIService>();
    builder.Services.AddScoped<ClaudeService>();
    builder.Services.AddScoped<IAIService>(sp =>
    {
        var httpContext = sp.GetRequiredService<IHttpContextAccessor>();
        var provider = httpContext.HttpContext?.Request.Headers["X-Provider"].ToString()?.ToLower();
        
        return provider switch
        {
            "claude" => sp.GetRequiredService<ClaudeService>(),
            _ => sp.GetRequiredService<OpenAIService>()
        };
    });

    // Add HttpContextAccessor for request context
    builder.Services.AddHttpContextAccessor();

    var app = builder.Build();

    // Apply migrations on startup (skip in test environment)
    if (!builder.Environment.IsEnvironment("Testing"))
    {
        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while migrating the database");
                throw;
            }
        }
    }

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(c =>
        {
            // This sets up the JSON endpoint at /api/swagger/v1/swagger.json
            c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
        });
        
        app.UseSwaggerUI(c =>
        {
            // This sets up the Swagger UI at /api/swagger
            c.RoutePrefix = "api/swagger";
            c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "API v1");
        });
    }

    // Important: UseForwardedHeaders must be called before other middleware
    app.UseForwardedHeaders();

    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthorization();
    app.MapControllers();
    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }