using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Hosting;
using Api.Data;

namespace Api.Tests;

public class TestBase : IDisposable
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;
    private readonly SqliteConnection _connection;
    
    public TestBase()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices((context, services) =>
                {
                    // Set environment to prevent migrations
                    context.HostingEnvironment.EnvironmentName = "Testing";
                    
                    // Remove the app's DbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add SQLite with the open connection
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseSqlite(_connection);
                    });

                    // Create a new service provider
                    var serviceProvider = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database context
                    using var scope = serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    try
                    {
                        // Ensure the database is created with the latest schema
                        db.Database.EnsureCreated();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("An error occurred while creating the test database.", ex);
                    }
                });
            });

        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
        _connection.Dispose();
    }
} 