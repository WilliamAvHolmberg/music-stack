using Microsoft.Playwright;

namespace Api.WebScraping;

public interface IWebScraperService
{
    Task<IBrowser> InitializeBrowserAsync();
    Task<(string Html, string Screenshot, double LoadTime)> ScrapeUrlAsync(string url);
} 