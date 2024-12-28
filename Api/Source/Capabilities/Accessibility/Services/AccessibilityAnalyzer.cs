using HtmlAgilityPack;
using Api.Accessibility.Models;
using Api.Accessibility.Services.Rules;
using Api.WebScraping;
using Microsoft.Playwright;

namespace Api.Accessibility.Services;

public class AccessibilityAnalyzer : IAccessibilityAnalyzer
{
    private readonly IWebScraperService _scraper;
    private readonly ILogger<AccessibilityAnalyzer> _logger;

    public AccessibilityAnalyzer(
        IWebScraperService scraper,
        ILogger<AccessibilityAnalyzer> logger)
    {
        _scraper = scraper;
        _logger = logger;
    }

    public async Task<AnalysisResult> AnalyzeUrlAsync(string url)
    {
        var issues = new List<AccessibilityIssue>();
        var processedElements = new HashSet<string>();
        var browser = await _scraper.InitializeBrowserAsync();

        try
        {
            var page = await browser.NewPageAsync();
            await page.GotoAsync(url, new() { WaitUntil = WaitUntilState.NetworkIdle });

            foreach (var criterion in AccessibilityCriteria.All)
            {
                try
                {
                    foreach (var selector in criterion.ValidationRules.Selectors)
                    {
                        // Convert XPath to CSS if needed
                        var cssSelector = selector.StartsWith("//") 
                            ? ConvertXPathToCSS(selector) 
                            : selector;

                        // Get all matching elements
                        var elements = await page.QuerySelectorAllAsync(cssSelector);
                        
                        foreach (var element in elements)
                        {
                            foreach (var rule in criterion.ValidationRules.Rules)
                            {
                                var (isValid, currentValue) = await ValidateElementRule(page, element, rule);
                                if (!isValid)
                                {
                                    var elementContext = await GetElementContext(element);
                                    if (!string.IsNullOrWhiteSpace(elementContext) && !processedElements.Contains(elementContext))
                                    {
                                        processedElements.Add(elementContext);
                                        issues.Add(new AccessibilityIssue(
                                            Section: criterion.Section,
                                            Description: $"{criterion.Title}: {criterion.Description}",
                                            Element: elementContext,
                                            Impact: criterion.Impact,
                                            ScoreImpact: criterion.ScoreImpact,
                                            Suggestion: GetSuggestionForRule(rule),
                                            CurrentValue: currentValue,
                                            RuleType: rule.Type
                                        ));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error applying criterion {Section}", criterion.Section);
                }
            }

            await page.CloseAsync();
        }
        finally
        {
            // Browser cleanup is handled by scraper service
        }

        var score = CalculateScore(issues);
        
        // First group by section
        var sections = issues
            .GroupBy(i => i.Section)
            .Select(sectionGroup =>
            {
                var firstIssue = sectionGroup.First();
                var criterion = AccessibilityCriteria.All.First(c => c.Section == firstIssue.Section);
                
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

                return new AccessibilitySection(
                    Section: criterion.Section,
                    Title: criterion.Title,
                    Description: criterion.Description,
                    Impact: criterion.Impact,
                    TotalIssues: sectionGroup.Count(),
                    ScoreImpact: sectionGroup.Sum(i => i.ScoreImpact),
                    IssueGroups: issueGroups
                );
            })
            .OrderByDescending(s => Math.Abs(s.ScoreImpact))
            .ToList();

        return new AnalysisResult(sections, issues, score);
    }

    private async Task<(bool isValid, string currentValue)> ValidateElementRule(IPage page, IElementHandle element, Rule rule)
    {
        try 
        {
            // Skip text-related rules for elements without text
            if ((rule.Type == "fontSize" || rule.Type == "lineHeight"))
            {
                var hasText = await HasMeaningfulText(element);
                if (!hasText)
                {
                    _logger.LogInformation("Skipping {RuleType} validation for element without meaningful text", rule.Type);
                    return (true, string.Empty);
                }
            }

            return rule.Type switch
            {
                "fontSize" => await ValidateFontSize(page, element, rule),
                "lineHeight" => await ValidateLineHeight(page, element, rule),
                "contrast" => await ValidateContrast(page, element, rule),
                "targetSize" => await ValidateTargetSize(page, element, rule),
                "spacing" => await ValidateSpacing(page, element, rule),
                "labelPresence" => await ValidateLabelPresence(page, element),
                "ariaLabel" => await ValidateAriaLabel(element),
                _ => (true, string.Empty)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate rule {RuleType}", rule.Type);
            return (false, "error");
        }
    }

    private async Task<(bool isValid, string currentValue)> ValidateFontSize(IPage page, IElementHandle element, Rule rule)
    {
        try
        {
            var fontSize = await page.EvaluateAsync<double>(@"(element) => {
                const style = window.getComputedStyle(element);
                return parseFloat(style.fontSize);
            }", element);

            return (fontSize >= rule.Min, $"{fontSize}px");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate font size");
            return (false, "unknown");
        }
    }

    private async Task<(bool isValid, string currentValue)> ValidateLineHeight(IPage page, IElementHandle element, Rule rule)
    {
        try
        {
            var lineHeight = await page.EvaluateAsync<double>(@"(element) => {
                const style = window.getComputedStyle(element);
                const lineHeight = style.lineHeight;
                if (lineHeight === 'normal') return 1.2;
                return parseFloat(lineHeight);
            }", element);

            return (lineHeight >= rule.Min, lineHeight.ToString("F1"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate line height");
            return (false, "unknown");
        }
    }

    private async Task<(bool isValid, string currentValue)> ValidateTargetSize(IPage page, IElementHandle element, Rule rule)
    {
        var size = await page.EvaluateAsync<(double width, double height)>(@"(element) => {
            const rect = element.getBoundingClientRect();
            return { width: rect.width, height: rect.height };
        }", element);

        return (
            size.width >= rule.Min && size.height >= rule.Min,
            $"{size.width}x{size.height}px"
        );
    }

    private async Task<(bool isValid, string currentValue)> ValidateSpacing(IPage page, IElementHandle element, Rule rule)
    {
        var spacing = await page.EvaluateAsync<(double margin, double padding)>(@"(element) => {
            const style = window.getComputedStyle(element);
            const margin = parseFloat(style.marginTop) + parseFloat(style.marginBottom);
            const padding = parseFloat(style.paddingTop) + parseFloat(style.paddingBottom);
            return { margin, padding };
        }", element);

        return (
            spacing.margin >= rule.Min || spacing.padding >= rule.Min,
            $"margin: {spacing.margin}px, padding: {spacing.padding}px"
        );
    }

    private async Task<(bool isValid, string currentValue)> ValidateLabelPresence(IPage page, IElementHandle element)
    {
        var hasLabel = await page.EvaluateAsync<bool>(@"(element) => {
            if (element.tagName.toLowerCase() !== 'input') return true;
            const id = element.id;
            if (!id) return false;
            return !!document.querySelector(`label[for='${id}']`);
        }", element);

        return (hasLabel, "missing");
    }

    private async Task<(bool isValid, string currentValue)> ValidateAriaLabel(IElementHandle element)
    {
        var ariaLabel = await element.GetAttributeAsync("aria-label");
        return (!string.IsNullOrEmpty(ariaLabel), ariaLabel);
    }

    private async Task<(bool isValid, string currentValue)> ValidateContrast(IPage page, IElementHandle element, Rule rule)
    {
        var colors = await page.EvaluateAsync<(string fg, string bg)>(@"(element) => {
            const style = window.getComputedStyle(element);
            return {
                fg: style.color,
                bg: style.backgroundColor
            };
        }", element);

        // Here we would calculate the actual contrast ratio
        // For now, return true
        return (true, $"fg: {colors.fg}, bg: {colors.bg}");
    }

    private string GetSuggestionForRule(Rule rule)
    {
        return rule.Type switch
        {
            "fontSize" => $"Increase font size to at least {rule.Min}px",
            "lineHeight" => $"Increase line height to at least {rule.Min}",
            "contrast" => "Ensure sufficient color contrast",
            "targetSize" => $"Increase target size to at least {rule.Min}px",
            "spacing" => $"Increase spacing to at least {rule.Min}px",
            "labelPresence" => "Add a label element",
            "ariaLabel" => "Add an aria-label attribute",
            _ => "Fix accessibility issue"
        };
    }

    private double CalculateScore(List<AccessibilityIssue> issues)
    {
        var baseScore = 100.0;
        var totalImpact = issues.Sum(i => i.ScoreImpact);
        return Math.Max(0, Math.Min(100, baseScore + totalImpact));
    }

    private string ConvertXPathToCSS(string xpath)
    {
        // Simple conversion for common XPath patterns
        return xpath switch
        {
            "//p" => "p",
            "//span" => "span",
            "//div" => "div",
            "//a" => "a",
            "//button" => "button",
            "//label" => "label",
            "//input" => "input",
            "//form" => "form",
            "//select" => "select",
            "//textarea" => "textarea",
            "//*[@role='button']" => "*[role='button']",
            _ => xpath.Replace("//", "")
        };
    }

    private async Task<bool> HasMeaningfulText(IElementHandle element)
    {
        try
        {
            // Check if element is visible
            var isVisible = await element.IsVisibleAsync();
            if (!isVisible)
            {
                _logger.LogInformation("Element is not visible");
                return false;
            }

            // Check if element is meaningful
            var isMeaningful = await IsMeaningfulElement(element);
            if (!isMeaningful)
            {
                _logger.LogInformation("Element is not meaningful");
                return false;
            }

            // Get text content
            var text = await element.TextContentAsync();
            var hasText = !string.IsNullOrWhiteSpace(text);

            _logger.LogInformation(
                "Element text check: Text='{Text}', IsVisible={IsVisible}", 
                text ?? "(empty)", 
                isVisible);

            return hasText;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check if element has meaningful text");
            return false;
        }
    }

    private async Task<bool> IsMeaningfulElement(IElementHandle element)
    {
        try
        {
            return await element.EvaluateAsync<bool>(@"(element) => {
                const tagName = element.tagName.toLowerCase();
                
                // Skip pure container/structural elements
                if (tagName === 'div' || tagName === 'span' || tagName === 'section') {
                    // Check if it's decorative
                    const isDecorative = 
                        element.getAttribute('aria-hidden') === 'true' ||
                        element.getAttribute('role') === 'presentation' ||
                        element.classList.contains('gatsby-image-wrapper') ||
                        element.classList.contains('gatsby-background-image-wrapper') ||
                        element.classList.contains('material-icons');

                    if (isDecorative) return false;

                    // Check computed style for decorative properties
                    const style = window.getComputedStyle(element);
                    if (style.opacity === '0' ||
                        style.display === 'none' ||
                        style.visibility === 'hidden' ||
                        (style.width === '0px' && style.height === '0px')) {
                        return false;
                    }

                    // Only include if it has direct text content or ARIA attributes
                    const hasDirectText = Array.from(element.childNodes)
                        .some(node => node.nodeType === 3 && node.textContent.trim().length > 0);
                    
                    return hasDirectText || 
                           element.hasAttribute('aria-label') ||
                           element.hasAttribute('aria-description');
                }
                
                return true;
            }");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check if element is meaningful");
            return false;
        }
    }

    private string GetRuleTypeFromDescription(string description)
    {
        if (description.Contains("font size", StringComparison.OrdinalIgnoreCase)) return "Font Size";
        if (description.Contains("line height", StringComparison.OrdinalIgnoreCase)) return "Line Height";
        if (description.Contains("contrast", StringComparison.OrdinalIgnoreCase)) return "Contrast";
        if (description.Contains("target size", StringComparison.OrdinalIgnoreCase)) return "Target Size";
        if (description.Contains("spacing", StringComparison.OrdinalIgnoreCase)) return "Spacing";
        if (description.Contains("label", StringComparison.OrdinalIgnoreCase)) return "Label";
        if (description.Contains("aria", StringComparison.OrdinalIgnoreCase)) return "ARIA";
        return "Other";
    }

    private async Task<string> GetElementContext(IElementHandle element)
    {
        try
        {
            var outerHtml = await element.EvaluateAsync<string>("el => el.outerHTML");
            const int maxLength = 200;
            
            if (outerHtml.Length > maxLength)
            {
                return outerHtml.Substring(0, maxLength) + "...";
            }
            
            return outerHtml;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get element context");
            return string.Empty;
        }
    }
} 