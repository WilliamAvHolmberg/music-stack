using Api.AI;
using Api.W3;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests.Services;

public class W3ServiceTests : TestBase
{
    private readonly IW3Service _w3Service;
    
    public W3ServiceTests()
    {
        var httpClient = _factory.Services.GetRequiredService<IHttpClientFactory>().CreateClient();
        _w3Service = new W3Service(httpClient);
    }
    
    [Fact]
    public async Task ValidateHtmlAsync_ValidHtml_ReturnsNoErrors()
    {
        // Arrange
        var html = @"<!DOCTYPE html>
            <html>
            <head><title>Test</title></head>
            <body>Valid HTML</body>
            </html>";
        
        // Act
        var result = await _w3Service.ValidateHtmlAsync(html);
        
        // Assert
        Assert.Empty(result.Messages.Where(m => m.Type == "error"));
    }
    
    [Fact]
    public async Task ValidateHtmlAsync_InvalidHtml_ReturnsErrors()
    {
        // Arrange
        var html = "<body>Invalid HTML</body>";
        
        // Act
        var result = await _w3Service.ValidateHtmlAsync(html);
        
        // Assert
        Assert.Contains(result.Messages, m => m.Type == "error");
    }
    
    [Fact]
    public async Task ValidateHtmlAsync_EmptyHtml_ReturnsErrors()
    {
        // Arrange
        var html = "";
        
        // Act
        var result = await _w3Service.ValidateHtmlAsync(html);
        
        // Assert
        Assert.Contains(result.Messages, m => m.Type == "error");
    }
    
    [Fact]
    public async Task ValidateHtmlAsync_MalformedHtml_ReturnsErrors()
    {
        // Arrange
        var html = "<div>Unclosed div";
        
        // Act
        var result = await _w3Service.ValidateHtmlAsync(html);
        
        // Assert
        Assert.Contains(result.Messages, m => m.Type == "error");
    }
} 