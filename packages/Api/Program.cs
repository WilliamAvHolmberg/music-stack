using Serilog;
using Api.AI;
using Api.Configuration;
using Microsoft.EntityFrameworkCore;
using Api.Shared.Infrastructure.Database;
using Microsoft.AspNetCore.HttpOverrides;
using System.IO;
using Api.Domain.Songs.Services;
using Api.Domain.Games.Services;
using Api.Domain.Games.Hubs;

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
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });

    // Add SQLite
    var dbPath = builder.Environment.IsProduction() 
        ? Path.Combine(Environment.GetEnvironmentVariable("DEPLOY_PATH") ?? "", settings.Database.SqlitePath)
        : settings.Database.SqlitePath;

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite($"Data Source={dbPath}"));

    // Add HttpClient
    builder.Services.AddHttpClient();

    // Add domain services
    builder.Services.AddScoped<ISongService, SongService>();
    builder.Services.AddScoped<IGameService, GameService>();
    builder.Services.AddScoped<IGameTemplateService, GameTemplateService>();

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

    // Add SignalR
    builder.Services.AddSignalR();

    // Add notification service
    builder.Services.AddScoped<IGameNotificationService, GameNotificationService>();

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
            c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
        });
        
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "api/swagger";
            c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "API v1");
        });
    }

    app.UseForwardedHeaders();
    app.UseHttpsRedirection();
    app.MapHub<GameHub>("/api/gamehub");
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