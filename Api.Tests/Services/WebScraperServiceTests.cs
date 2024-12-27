using Api.WebScraping;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Xunit;

namespace Api.Tests.Services;

public class WebScraperServiceTests : IAsyncLifetime
{
    private readonly IWebScraperService _scraper;
    private readonly ILogger<PlaywrightScraperService> _logger;

    public WebScraperServiceTests()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = loggerFactory.CreateLogger<PlaywrightScraperService>();
        _scraper = new PlaywrightScraperService(_logger);
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ScrapeUrlAsync_ValidUrl_ReturnsHtml()
    {
        // Arrange
        var url = "https://styrsohafsbad.se";

        // Act
        var result = await _scraper.ScrapeUrlAsync(url);

        // Assert
        Assert.NotEmpty(result.Html);
        Assert.True(result.LoadTime > 0);
    }

    [Fact]
    public async Task ScrapeUrlAsync_InvalidUrl_ThrowsException()
    {
        // Arrange
        var url = "https://this-domain-definitely-does-not-exist-123456789.com/";

        // Act & Assert
        await Assert.ThrowsAsync<PlaywrightException>(() => _scraper.ScrapeUrlAsync(url));
    }
} 