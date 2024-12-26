using Microsoft.AspNetCore.Mvc;
using Api.WebScraping.Models.Requests;
using Api.WebScraping.Models.Responses;

namespace Api.WebScraping.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebScraperController : ControllerBase
{
    private readonly IWebScraperService _scraperService;
    private readonly ILogger<WebScraperController> _logger;

    public WebScraperController(
        IWebScraperService scraperService,
        ILogger<WebScraperController> logger)
    {
        _scraperService = scraperService;
        _logger = logger;
    }

    [HttpPost("scrape")]
    public async Task<ActionResult<ScrapeUrlResponse>> ScrapeUrl([FromBody] ScrapeUrlRequest request)
    {
        try
        {
            var (html, screenshot, loadTime) = await _scraperService.ScrapeUrlAsync(request.Url);
            
            return Ok(new ScrapeUrlResponse(
                Html: html,
                Screenshot: screenshot,
                LoadTimeSeconds: loadTime
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to scrape URL: {Url}", request.Url);
            return StatusCode(500, "Failed to scrape URL");
        }
    }
} 