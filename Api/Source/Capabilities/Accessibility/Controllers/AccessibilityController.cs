using Microsoft.AspNetCore.Mvc;
using Api.Accessibility.Models;
using Api.Accessibility.Services;

namespace Api.Accessibility.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccessibilityController : ControllerBase
{
    private readonly IAccessibilityAnalyzer _analyzer;
    private readonly ILogger<AccessibilityController> _logger;

    public AccessibilityController(
        IAccessibilityAnalyzer analyzer,
        ILogger<AccessibilityController> logger)
    {
        _analyzer = analyzer;
        _logger = logger;
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<AnalysisResult>> AnalyzeUrl([FromBody] AnalyzeUrlRequest request)
    {
        try
        {
            var result = await _analyzer.AnalyzeUrlAsync(request.Url);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to analyze URL: {Url}", request.Url);
            return StatusCode(500, "Failed to analyze URL");
        }
    }
} 