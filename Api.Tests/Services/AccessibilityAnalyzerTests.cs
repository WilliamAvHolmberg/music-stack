using Api.Accessibility.Services;
using Api.WebScraping;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Api.Tests.Services;

public class AccessibilityAnalyzerTests : IAsyncLifetime
{
    private readonly IAccessibilityAnalyzer _analyzer;
    private readonly IWebScraperService _scraper;
    private readonly ILogger<AccessibilityAnalyzer> _logger;

    public AccessibilityAnalyzerTests()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = loggerFactory.CreateLogger<AccessibilityAnalyzer>();
        _scraper = new PlaywrightScraperService(loggerFactory.CreateLogger<PlaywrightScraperService>());
        _analyzer = new AccessibilityAnalyzer(_scraper, _logger);
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task AnalyzeUrlAsync_ShouldNotReportEmptyElements()
    {
        // Arrange
        var url = "https://styrsohafsbad.se";

        // Act
        var result = await _analyzer.AnalyzeUrlAsync(url);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.DetailedIssues);

        // No issues should have empty elements
        var emptyElementIssues = result.DetailedIssues.Where(i => string.IsNullOrWhiteSpace(i.Element)).ToList();
        Assert.Empty(emptyElementIssues);
    }

    [Fact]
    public async Task AnalyzeUrlAsync_ShouldGroupSimilarIssues()
    {
        // Arrange
        var url = "https://styrsohafsbad.se";

        // Act
        var result = await _analyzer.AnalyzeUrlAsync(url);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Sections);
        
        // Each section should have issue groups
        foreach (var section in result.Sections)
        {
            Assert.True(section.TotalIssues > 0, "Section should have issues");
            Assert.NotEmpty(section.IssueGroups);
            Assert.Equal(section.TotalIssues, section.IssueGroups.Sum(g => g.Count));
            
            // Each group should have examples
            foreach (var group in section.IssueGroups)
            {
                Assert.True(group.Count > 0, "Group should have a count");
                Assert.NotEmpty(group.Examples);
                Assert.True(group.Examples.Count <= 3, "Should have at most 3 examples");
                
                // Count should match number of detailed issues with same section and rule type
                var matchingDetailedIssues = result.DetailedIssues.Count(i => 
                    i.Section == section.Section && 
                    i.RuleType == group.RuleType);
                Assert.Equal(group.Count, matchingDetailedIssues);
            }
        }

        // Log sections for inspection
        foreach (var section in result.Sections)
        {
            _logger.LogInformation(
                "Section: {Section} - {Title}\nDescription: {Description}\nTotal Issues: {TotalIssues}\nImpact: {Impact}\n",
                section.Section,
                section.Title,
                section.Description,
                section.TotalIssues,
                section.Impact);

            foreach (var group in section.IssueGroups)
            {
                _logger.LogInformation(
                    "  Rule Type: {RuleType}\n  Count: {Count}\n  Suggestion: {Suggestion}\n  Examples: {Examples}",
                    group.RuleType,
                    group.Count,
                    group.Suggestion,
                    string.Join("\n", group.Examples.Select(e => $"\n    - {e}")));
            }
        }
    }

    [Fact]
    public async Task AnalyzeUrlAsync_ShouldIgnoreDecorativeElements()
    {
        // Arrange
        var url = "https://styrsohafsbad.se";

        // Act
        var result = await _analyzer.AnalyzeUrlAsync(url);

        // Assert
        Assert.NotNull(result);
        
        // Log all issues for inspection
        foreach (var issue in result.DetailedIssues)
        {
            _logger.LogInformation(
                "Found issue: Section={Section}, Type={Type}, Value={Value}, Element={Element}",
                issue.Section,
                issue.RuleType,
                issue.CurrentValue,
                issue.Element);
        }
        
        // Should not have issues for purely decorative elements
        var decorativeIssues = result.DetailedIssues
            .Where(i => i.Section == "EAA.1.1" && 
                       (i.Element.Contains("opacity:0") ||
                        i.Element.Contains("display:none") ||
                        i.Element.Contains("visibility:hidden") ||
                        i.Element.Contains("aria-hidden=\"true\"") ||
                        i.Element.Contains("role=\"presentation\"")))
            .ToList();

        Assert.Empty(decorativeIssues);

        // Should find issues for real text elements
        var textIssues = result.DetailedIssues
            .Where(i => i.Section == "EAA.1.1" && 
                       i.RuleType == "fontSize" &&
                       !string.IsNullOrWhiteSpace(i.CurrentValue) &&
                       !string.IsNullOrWhiteSpace(i.Element))
            .ToList();

        Assert.NotEmpty(textIssues);
        
        foreach (var issue in textIssues)
        {
            _logger.LogInformation(
                "Found text issue: Element={Element}, FontSize={FontSize}", 
                issue.Element, 
                issue.CurrentValue);
        }
    }

    [Fact]
    public async Task AnalyzeUrlAsync_ShouldNotSplitTextIntoChunks()
    {
        // Arrange
        var url = "https://styrsohafsbad.se";

        // Act
        var result = await _analyzer.AnalyzeUrlAsync(url);

        // Assert
        Assert.NotNull(result);

        // Check for suspiciously short text elements (likely chunks)
        var shortTextIssues = result.DetailedIssues
            .Where(i => !string.IsNullOrEmpty(i.Element) && 
                       i.Element.Length < 10 && 
                       !i.Element.Contains("<") && // Not an HTML tag
                       !i.Element.Contains(">"))
            .ToList();

        Assert.Empty(shortTextIssues);
    }

    [Fact]
    public async Task AnalyzeUrlAsync_ShouldNotDuplicateIssuesForSameElement()
    {
        // Arrange
        var url = "https://styrsohafsbad.se";

        // Act
        var result = await _analyzer.AnalyzeUrlAsync(url);

        // Assert
        Assert.NotNull(result);

        // Group issues by element and check for duplicates
        var duplicateIssues = result.DetailedIssues
            .GroupBy(i => i.Element)
            .Where(g => g.Count() > 1 && !string.IsNullOrEmpty(g.Key))
            .ToList();

        Assert.Empty(duplicateIssues);
    }

    [Fact]
    public async Task AnalyzeUrlAsync_ShouldProvideUsefulContext()
    {
        // Arrange
        var url = "https://styrsohafsbad.se";

        // Act
        var result = await _analyzer.AnalyzeUrlAsync(url);

        // Assert
        Assert.NotNull(result);

        foreach (var section in result.Sections)
        {
            // Description should be specific
            Assert.True(
                section.Description.Length > 30,
                $"Description too vague: {section.Description}"
            );

            foreach (var group in section.IssueGroups)
            {
                // Suggestion should be actionable
                Assert.True(
                    group.Suggestion.Length > 20,
                    $"Suggestion not actionable: {group.Suggestion}"
                );

                // Examples should be meaningful
                foreach (var example in group.Examples)
                {
                    Assert.True(
                        example.Length > 20 || // Long enough to be meaningful
                        example.Contains("<") || // Contains HTML
                        example.Contains(">"), // Contains HTML
                        $"Example lacks context: {example}"
                    );
                }
            }
        }
    }

    [Fact]
    public async Task AnalyzeUrlAsync_ShouldHandleNestedElements()
    {
        // Arrange
        var url = "https://styrsohafsbad.se";

        // Act
        var result = await _analyzer.AnalyzeUrlAsync(url);

        // Assert
        Assert.NotNull(result);

        // Check that we're not just getting leaf nodes in examples
        var hasNestedElements = result.Sections
            .SelectMany(s => s.IssueGroups)
            .SelectMany(g => g.Examples)
            .Any(e => e.Contains(">") && 
                     e.Split(new[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries).Length > 2);

        Assert.True(hasNestedElements, "No nested elements found in analysis");
    }

    [Fact]
    public async Task AnalyzeUrlAsync_ShouldProvideConsistentScoring()
    {
        // Arrange
        var url = "https://styrsohafsbad.se";

        // Act
        var result1 = await _analyzer.AnalyzeUrlAsync(url);
        var result2 = await _analyzer.AnalyzeUrlAsync(url);

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);

        // Scores should be consistent between runs
        Assert.Equal(result1.Score, result2.Score);

        // Score should make sense relative to issues
        Assert.True(result1.Score >= 0 && result1.Score <= 100);
        
        // More high-impact issues should mean lower score
        var highImpactSections = result1.Sections.Count(s => s.Impact.Equals("High", StringComparison.OrdinalIgnoreCase));
        if (highImpactSections > 2)
        {
            Assert.True(result1.Score < 70);
        }
    }
} 