using Api.WebScraping;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Api.Tests.Services;

public class WebScraperServiceTests
{
    private readonly IWebScraperService _service;
    private readonly ILogger<PlaywrightScraperService> _logger;

    public WebScraperServiceTests()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = loggerFactory.CreateLogger<PlaywrightScraperService>();
        _service = new PlaywrightScraperService(_logger);
    }

    [Fact]
    public async Task ScrapeUrlAsync_ValidUrl_ReturnsContent()
    {
        // Arrange
        var url = "https://example.com";

        // Act
        var (html, screenshot, loadTime) = await _service.ScrapeUrlAsync(url);

        // Assert
        Assert.NotEmpty(html);
        Assert.NotEmpty(screenshot);
        Assert.True(loadTime > 0);
        Assert.Contains("<html", html);
        Assert.Contains("</html>", html);
    }

    [Fact]
    public async Task ScrapeUrlAsync_InvalidUrl_ThrowsException()
    {
        // Arrange
        var url = "https://this-domain-definitely-does-not-exist-123456789.com";

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.ScrapeUrlAsync(url));
    }
} 