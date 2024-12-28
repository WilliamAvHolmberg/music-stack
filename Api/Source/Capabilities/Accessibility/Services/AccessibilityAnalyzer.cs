using HtmlAgilityPack;
using Api.Accessibility.Models;
using Api.WebScraping;
using Microsoft.Playwright;

namespace Api.Accessibility.Services;

public class AccessibilityAnalyzer : IAccessibilityAnalyzer
{
    private readonly IWebScraperService _scraper;
    private readonly ILogger<AccessibilityAnalyzer> _logger;
    private readonly ISection1Service _section1Service;

    public AccessibilityAnalyzer(
        IWebScraperService scraper,
        ILogger<AccessibilityAnalyzer> logger,
        ISection1Service section1Service)
    {
        _scraper = scraper;
        _logger = logger;
        _section1Service = section1Service;
    }

    public async Task<AnalysisResult> AnalyzeUrlAsync(string url)
    {
        var issues = new List<AccessibilityIssue>();
        var browser = await _scraper.InitializeBrowserAsync();

        try
        {
            var page = await browser.NewPageAsync();
            await page.GotoAsync(url, new() { WaitUntil = WaitUntilState.NetworkIdle });

            // Analyze Section 1 (Perceivable)
            var allElements = await page.QuerySelectorAllAsync("*");
            foreach (var element in allElements)
            {
                try
                {
                    var elementIssues = await _section1Service.AnalyzeAsync(page, element);
                    issues.AddRange(elementIssues);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error analyzing element for Section 1");
                }
            }

            await page.CloseAsync();
        }
        finally
        {
            // Browser cleanup is handled by scraper service
        }

        var score = CalculateScore(issues);
        
        // Group by section
        var sections = issues
            .GroupBy(i => i.Section)
            .Select(sectionGroup =>
            {
                // Then group by rule type within each section
                var issueGroups = sectionGroup
                    .GroupBy(i => i.RuleType)
                    .Select(ruleGroup => new AccessibilityIssueGroup(
                        RuleType: ruleGroup.Key,
                        Count: ruleGroup.Count(),
                        Suggestion: ruleGroup.First().Suggestion,
                        Examples: ruleGroup.Take(3).Select(i => i.Element).ToList(),
                        CurrentValues: ruleGroup.Take(3).Select(i => i.CurrentValue).ToList()
                    ))
                    .OrderByDescending(g => g.Count)
                    .ToList();

                // Get section metadata from first issue
                var firstIssue = sectionGroup.First();
                return new AccessibilitySection(
                    Section: firstIssue.Section,
                    Title: GetSectionTitle(firstIssue.Section),
                    Description: GetSectionDescription(firstIssue.Section),
                    Impact: firstIssue.Impact,
                    TotalIssues: sectionGroup.Count(),
                    ScoreImpact: sectionGroup.Sum(i => i.ScoreImpact),
                    IssueGroups: issueGroups
                );
            })
            .OrderByDescending(s => Math.Abs(s.ScoreImpact))
            .ToList();

        return new AnalysisResult(sections, issues, score);
    }

    private double CalculateScore(List<AccessibilityIssue> issues)
    {
        var baseScore = 100.0;
        var totalImpact = issues.Sum(i => i.ScoreImpact);
        return Math.Max(0, Math.Min(100, baseScore + totalImpact));
    }

    private string GetSectionTitle(string section) => section switch
    {
        "EAA.1.1" => "Text Alternatives",
        "EAA.1.2" => "Time-based Media",
        "EAA.1.3" => "Adaptable Content",
        "EAA.1.4" => "Distinguishable Content",
        _ => "Accessibility Requirement"
    };

    private string GetSectionDescription(string section) => section switch
    {
        "EAA.1.1" => "Provide text alternatives for non-text content",
        "EAA.1.2" => "Provide alternatives for time-based media",
        "EAA.1.3" => "Create content that can be presented in different ways",
        "EAA.1.4" => "Make it easier for users to see and hear content",
        _ => "Web content must meet accessibility guidelines"
    };
} 