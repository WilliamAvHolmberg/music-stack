using Microsoft.Playwright;
using System.Diagnostics;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Api.WebScraping;

public class PlaywrightScraperService : IWebScraperService
{
    private readonly ILogger<PlaywrightScraperService> _logger;
    private IBrowser? _browser;

    public PlaywrightScraperService(ILogger<PlaywrightScraperService> logger)
    {
        _logger = logger;
    }

    private string SanitizeHtml(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Remove all script tags
        var scriptNodes = doc.DocumentNode.SelectNodes("//script");
        if (scriptNodes != null)
        {
            foreach (var node in scriptNodes)
            {
                node.Remove();
            }
        }

        // Remove base64 encoded images from img tags
        var imgNodes = doc.DocumentNode.SelectNodes("//img");
        if (imgNodes != null)
        {
            foreach (var node in imgNodes)
            {
                var src = node.GetAttributeValue("src", "");
                if (src.StartsWith("data:image"))
                {
                    node.SetAttributeValue("src", "#");
                }
            }
        }

        // Clean style tags
        var styleNodes = doc.DocumentNode.SelectNodes("//style");
        if (styleNodes != null)
        {
            foreach (var node in styleNodes)
            {
                var css = node.InnerHtml;
                // Remove data:image URLs from CSS
                css = Regex.Replace(css, @"url\(data:image[^)]+\)", "url(#)");
                node.InnerHtml = css;
            }
        }

        // Clean inline styles with background-image
        var nodesWithStyle = doc.DocumentNode.SelectNodes("//*[@style]");
        if (nodesWithStyle != null)
        {
            foreach (var node in nodesWithStyle)
            {
                var style = node.GetAttributeValue("style", "");
                if (style.Contains("data:image"))
                {
                    style = Regex.Replace(style, @"background-image:\s*url\(data:image[^)]+\)", "background-image:url(#)");
                    node.SetAttributeValue("style", style);
                }
            }
        }

        return doc.DocumentNode.OuterHtml;
    }

    public async Task<(string Html, string Screenshot, double LoadTime)> ScrapeUrlAsync(string url)
    {
        var browser = await InitializeBrowserAsync();
        var page = await browser.NewPageAsync();
        var stopwatch = new Stopwatch();

        try
        {
            stopwatch.Start();
            await page.GotoAsync(url, new() { WaitUntil = WaitUntilState.NetworkIdle });
            stopwatch.Stop();

            var html = await page.ContentAsync();
            var sanitizedHtml = SanitizeHtml(html);
            
            var screenshot = Convert.ToBase64String(
                await page.ScreenshotAsync(new() { FullPage = true })
            );

            return (sanitizedHtml, screenshot, stopwatch.Elapsed.TotalSeconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scraping URL: {Url}", url);
            throw;
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    public async Task<IBrowser> InitializeBrowserAsync()
    {
        if (_browser != null) return _browser;
        
        var playwright = await Playwright.CreateAsync();
        _browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = true
        });
        
        return _browser;
    }
} 