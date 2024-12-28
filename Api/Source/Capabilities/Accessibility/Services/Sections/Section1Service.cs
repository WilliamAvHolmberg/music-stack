using Microsoft.Playwright;
using Api.Accessibility.Models;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text.Json;

namespace Api.Accessibility.Services;

public class Section1Service : ISection1Service
{
    private readonly ILogger<Section1Service> _logger;
    private const string SECTION = "EAA.1";

    public Section1Service(ILogger<Section1Service> logger)
    {
        _logger = logger;
    }

    public async Task<List<AccessibilityIssue>> AnalyzeAsync(IPage page, IElementHandle element)
    {
        var issues = new List<AccessibilityIssue>();

        try
        {
            await AnalyzeTextAlternatives(page, element, issues);
            await AnalyzeTimeBasedMedia(page, element, issues);
            await AnalyzeAdaptableContent(page, element, issues);
            await AnalyzeDistinguishableContent(page, element, issues);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing Section 1");
        }

        return issues;
    }

    // 1.1 Text Alternatives (A)
    private async Task AnalyzeTextAlternatives(IPage page, IElementHandle element, List<AccessibilityIssue> issues)
    {
        try
        {
            var tagName = await element.EvaluateAsync<string>("el => el.tagName.toLowerCase()");
            var role = await element.GetAttributeAsync("role");
            
            // Handle img elements
            if (tagName == "img")
            {
                var altText = await element.GetAttributeAsync("alt");
                var src = await element.GetAttributeAsync("src") ?? "";
                var isDecorative = await IsDecorativeImage(element);
                var isComplex = await IsComplexImage(element, src);

                // Case 1: Decorative image without empty alt
                if (isDecorative && altText != "")
                {
                    var elementHtml = await GetElementContext(element);
                    issues.Add(new AccessibilityIssue(
                        Section: $"{SECTION}.1",
                        Description: "Decorative images should have empty alt text",
                        Element: elementHtml,
                        Impact: "Medium",
                        ScoreImpact: -8,
                        Suggestion: "Add alt=\"\" to decorative images",
                        CurrentValue: altText ?? "No alt",
                        RuleType: "altText"
                    ));
                }
                // Case 2: Complex image without proper description
                else if (isComplex && (!altText?.Contains(" ") ?? true))
                {
                    var elementHtml = await GetElementContext(element);
                    issues.Add(new AccessibilityIssue(
                        Section: $"{SECTION}.1",
                        Description: "Complex images need detailed descriptions",
                        Element: elementHtml,
                        Impact: "Critical",
                        ScoreImpact: -15,
                        Suggestion: "Add detailed alt text or aria-describedby for complex images",
                        CurrentValue: altText ?? "No alt",
                        RuleType: "complexImage"
                    ));
                }
                // Case 3: Regular image without alt
                else if (!isDecorative && string.IsNullOrWhiteSpace(altText))
                {
                    var elementHtml = await GetElementContext(element);
                    issues.Add(new AccessibilityIssue(
                        Section: $"{SECTION}.1",
                        Description: "Images must have alt text",
                        Element: elementHtml,
                        Impact: "Critical",
                        ScoreImpact: -15,
                        Suggestion: "Add descriptive alt text to the image",
                        CurrentValue: "No alt text",
                        RuleType: "altText"
                    ));
                }
            }
            
            // Handle background images
            var hasBackgroundImage = await HasBackgroundImage(page, element);
            if (hasBackgroundImage)
            {
                var hasTextAlternative = await HasTextAlternative(element);
                if (!hasTextAlternative)
                {
                    var elementHtml = await GetElementContext(element);
                    issues.Add(new AccessibilityIssue(
                        Section: $"{SECTION}.1",
                        Description: "Background images with meaning need text alternatives",
                        Element: elementHtml,
                        Impact: "High",
                        ScoreImpact: -10,
                        Suggestion: "Add aria-label or descriptive text content for meaningful background images",
                        CurrentValue: "No text alternative",
                        RuleType: "backgroundImage"
                    ));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing text alternatives");
        }
    }

    private async Task<bool> IsDecorativeImage(IElementHandle element)
    {
        try
        {
            // Check for decorative indicators
            var isDecorative = await element.EvaluateAsync<bool>(@"el => {
                const role = el.getAttribute('role');
                const className = el.className;
                return role === 'presentation' || 
                       role === 'none' ||
                       className.includes('decorative') ||
                       el.matches('[aria-hidden=""true""]') ||
                       el.matches('.gatsby-image-wrapper');
            }");

            // Check filename for decorative indicators
            var src = await element.GetAttributeAsync("src") ?? "";
            var decorativePattern = new Regex(@"(decoration|background|bg-|pattern|divider)", RegexOptions.IgnoreCase);
            
            return isDecorative || decorativePattern.IsMatch(src);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if image is decorative");
            return false;
        }
    }

    private async Task<bool> IsComplexImage(IElementHandle element, string src)
    {
        try
        {
            // Check filename for complexity indicators
            var complexPattern = new Regex(@"(chart|graph|diagram|map|infographic)", RegexOptions.IgnoreCase);
            if (complexPattern.IsMatch(src)) return true;

            // Check size - complex images tend to be larger
            var rect = await element.BoundingBoxAsync();
            if (rect != null && rect.Width > 300 && rect.Height > 300) return true;

            // Check for complex image indicators in class or ID
            return await element.EvaluateAsync<bool>(@"el => {
                const className = el.className.toLowerCase();
                const id = el.id.toLowerCase();
                return className.includes('chart') || 
                       className.includes('graph') ||
                       className.includes('diagram') ||
                       id.includes('chart') ||
                       id.includes('graph') ||
                       id.includes('diagram');
            }");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if image is complex");
            return false;
        }
    }

    private async Task<bool> HasBackgroundImage(IPage page, IElementHandle element)
    {
        try
        {
            return await page.EvaluateAsync<bool>(@"(el) => {
                const style = window.getComputedStyle(el);
                return style.backgroundImage !== 'none' && 
                       !style.backgroundImage.includes('gradient');
            }", element);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking for background image");
            return false;
        }
    }

    private async Task<bool> HasTextAlternative(IElementHandle element)
    {
        try
        {
            return await element.EvaluateAsync<bool>(@"el => {
                // Check for ARIA attributes
                if (el.hasAttribute('aria-label') || 
                    el.hasAttribute('aria-labelledby') ||
                    el.hasAttribute('aria-describedby')) {
                    return true;
                }

                // Check for visible text content
                const text = el.textContent?.trim();
                if (text && text.length > 0) {
                    return true;
                }

                // Check for title attribute
                if (el.hasAttribute('title')) {
                    return true;
                }

                return false;
            }");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking for text alternative");
            return false;
        }
    }

    // 1.2 Time-based Media (A)
    private async Task AnalyzeTimeBasedMedia(IPage page, IElementHandle element, List<AccessibilityIssue> issues)
    {
        try
        {
            var tagName = await element.EvaluateAsync<string>("el => el.tagName.toLowerCase()");
            
            if (tagName == "video" || tagName == "audio")
            {
                var hasControls = await element.GetAttributeAsync("controls");
                var hasCaptions = await element.QuerySelectorAsync("track[kind='captions']") != null;
                var hasAudioDescription = await element.QuerySelectorAsync("track[kind='descriptions']") != null;
                var isLive = await element.GetAttributeAsync("data-live") == "true";
                var hasTranscript = await HasTranscript(element);

                // Check for basic controls
                if (string.IsNullOrWhiteSpace(hasControls))
                {
                    var elementHtml = await GetElementContext(element);
                    issues.Add(new AccessibilityIssue(
                        Section: $"{SECTION}.2",
                        Description: "Media must have playback controls",
                        Element: elementHtml,
                        Impact: "High",
                        ScoreImpact: -10,
                        Suggestion: "Add controls attribute to media element",
                        CurrentValue: "No controls",
                        RuleType: "mediaControls"
                    ));
                }

                // Check for captions in video content
                if (!hasCaptions && tagName == "video")
                {
                    var elementHtml = await GetElementContext(element);
                    issues.Add(new AccessibilityIssue(
                        Section: $"{SECTION}.2",
                        Description: isLive ? "Live videos must have real-time captions" : "Videos must have captions",
                        Element: elementHtml,
                        Impact: "Critical",
                        ScoreImpact: -15,
                        Suggestion: isLive ? "Provide real-time captions for live content" : "Add captions using track element",
                        CurrentValue: "No captions",
                        RuleType: isLive ? "liveCaptions" : "captions"
                    ));
                }

                // Check for audio descriptions in video content
                if (!hasAudioDescription && tagName == "video")
                {
                    var elementHtml = await GetElementContext(element);
                    issues.Add(new AccessibilityIssue(
                        Section: $"{SECTION}.2",
                        Description: "Videos with visual information must have audio descriptions",
                        Element: elementHtml,
                        Impact: "High",
                        ScoreImpact: -10,
                        Suggestion: "Add audio descriptions using track[kind='descriptions']",
                        CurrentValue: "No audio descriptions",
                        RuleType: "audioDescription"
                    ));
                }

                // Check for transcript
                if (!hasTranscript)
                {
                    var elementHtml = await GetElementContext(element);
                    issues.Add(new AccessibilityIssue(
                        Section: $"{SECTION}.2",
                        Description: "Pre-recorded media should have a transcript",
                        Element: elementHtml,
                        Impact: "High",
                        ScoreImpact: -10,
                        Suggestion: "Provide a transcript for pre-recorded media content",
                        CurrentValue: "No transcript",
                        RuleType: "transcript"
                    ));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing time-based media");
        }
    }

    private async Task<bool> HasTranscript(IElementHandle element)
    {
        try
        {
            // Check for common transcript indicators
            return await element.EvaluateAsync<bool>(@"el => {
                // Check for aria-describedby pointing to transcript
                const describedBy = el.getAttribute('aria-describedby');
                if (describedBy) {
                    const transcriptEl = document.getElementById(describedBy);
                    if (transcriptEl?.textContent.length > 50) return true;
                }

                // Check for adjacent transcript
                const next = el.nextElementSibling;
                if (next?.classList.contains('transcript') || 
                    next?.getAttribute('data-type') === 'transcript' ||
                    next?.id.includes('transcript')) {
                    return true;
                }

                // Check for transcript link
                const container = el.closest('div, section, article');
                if (container) {
                    const transcriptLink = container.querySelector('a[href*=""transcript""]');
                    if (transcriptLink) return true;
                }

                return false;
            }");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking for transcript");
            return false;
        }
    }

    // 1.3 Adaptable (A)
    private async Task AnalyzeAdaptableContent(IPage page, IElementHandle element, List<AccessibilityIssue> issues)
    {
        try
        {
            var tagName = await element.EvaluateAsync<string>("el => el.tagName.toLowerCase()");

            // Check table structure
            if (tagName == "table")
            {
                _logger.LogInformation("Found table element");
                var isDataTable = await element.EvaluateAsync<bool>(@"el => {
                    // Check if table has any data attributes
                    if (el.dataset && Object.keys(el.dataset).length > 0) return true;

                    // Check if table has a caption
                    if (el.querySelector('caption')) return true;

                    // Check if table has a summary attribute
                    if (el.hasAttribute('summary')) return true;

                    // Check if table has any cells with numeric content
                    const cells = Array.from(el.querySelectorAll('td'));
                    const hasNumericCells = cells.some(cell => {
                        const text = cell.textContent.trim();
                        return /\d/.test(text) && text.length < 20; // Short text with numbers is likely data
                    });
                    if (hasNumericCells) return true;

                    // Check if table has multiple rows with similar structure
                    const rows = Array.from(el.rows);
                    if (rows.length > 1) {
                        const firstRowCellCount = rows[0].cells.length;
                        const allRowsSameStructure = rows.every(row => row.cells.length === firstRowCellCount);
                        // If all rows have the same number of cells and there are at least 2 cells per row,
                        // or if there are multiple rows with consistent structure, it's likely a data table
                        if (allRowsSameStructure && (firstRowCellCount > 1 || rows.length > 2)) {
                            console.log('Found data table structure:', { rows: rows.length, cells: firstRowCellCount });
                            return true;
                        }
                    }

                    // Check if table has ARIA table attributes
                    if (el.getAttribute('role') === 'grid' || 
                        el.getAttribute('role') === 'treegrid' ||
                        el.querySelector('[role=""columnheader""], [role=""rowheader""]')) {
                        return true;
                    }

                    return false;
                }");

                _logger.LogInformation($"isDataTable: {isDataTable}");

                if (isDataTable)
                {
                    _logger.LogInformation("Checking headers for data table");
                    var headerInfo = await element.EvaluateAsync<Dictionary<string, int>>(@"el => {
                        return {
                            thCount: el.querySelectorAll('th').length,
                            scopeCount: el.querySelectorAll('[scope]').length,
                            ariaHeaderCount: el.querySelectorAll('[role=""columnheader""], [role=""rowheader""]').length,
                            theadCount: el.querySelectorAll('thead').length
                        };
                    }");

                    var hasHeaders = headerInfo != null && (
                        headerInfo.GetValueOrDefault("thCount", 0) > 0 || 
                        headerInfo.GetValueOrDefault("scopeCount", 0) > 0 || 
                        headerInfo.GetValueOrDefault("ariaHeaderCount", 0) > 0 || 
                        headerInfo.GetValueOrDefault("theadCount", 0) > 0
                    );

                    _logger.LogInformation($"hasHeaders: {hasHeaders}");

                    if (!hasHeaders)
                    {
                        var elementHtml = await GetElementContext(element);
                        _logger.LogInformation("Adding table structure issue");
                        issues.Add(new AccessibilityIssue(
                            Section: "EAA.1.3",
                            Description: "Data tables must have proper headers",
                            Element: elementHtml,
                            Impact: "High",
                            ScoreImpact: -10,
                            Suggestion: "Add table headers using th elements or appropriate ARIA roles",
                            CurrentValue: $"No table headers found (th: {headerInfo.GetValueOrDefault("thCount", 0)}, scope: {headerInfo.GetValueOrDefault("scopeCount", 0)}, aria: {headerInfo.GetValueOrDefault("ariaHeaderCount", 0)}, thead: {headerInfo.GetValueOrDefault("theadCount", 0)})",
                            RuleType: "tableStructure"
                        ));
                    }
                }
            }

            // Check meaningful sequence
            var hasSequenceIssue = await HasSequenceIssue(element);
            if (hasSequenceIssue)
            {
                var elementHtml = await GetElementContext(element);
                issues.Add(new AccessibilityIssue(
                    Section: $"{SECTION}.3",
                    Description: "Content must have a meaningful reading sequence",
                    Element: elementHtml,
                    Impact: "High",
                    ScoreImpact: -10,
                    Suggestion: "Ensure content order matches visual presentation",
                    CurrentValue: "Incorrect content sequence",
                    RuleType: "meaningfulSequence"
                ));
            }

            // Check orientation restrictions
            var hasOrientationRestriction = await HasOrientationRestriction(page, element);
            if (hasOrientationRestriction)
            {
                var elementHtml = await GetElementContext(element);
                issues.Add(new AccessibilityIssue(
                    Section: $"{SECTION}.3",
                    Description: "Content must not be restricted to a single display orientation",
                    Element: elementHtml,
                    Impact: "Medium",
                    ScoreImpact: -8,
                    Suggestion: "Remove orientation restrictions from content",
                    CurrentValue: "Orientation locked",
                    RuleType: "orientation"
                ));
            }

            // Check semantic structure (existing check)
            var hasProperStructure = await element.EvaluateAsync<bool>(@"el => {
                const role = el.getAttribute('role');
                const tag = el.tagName.toLowerCase();
                
                return tag.match(/^(article|aside|footer|header|nav|main|section)$/) !== null || 
                       (role && ['banner', 'main', 'contentinfo', 'complementary', 'navigation'].includes(role));
            }");

            if (!hasProperStructure)
            {
                var elementHtml = await GetElementContext(element);
                issues.Add(new AccessibilityIssue(
                    Section: $"{SECTION}.3",
                    Description: "Content should use proper semantic structure",
                    Element: elementHtml,
                    Impact: "Medium",
                    ScoreImpact: -8,
                    Suggestion: "Use semantic HTML elements or appropriate ARIA roles",
                    CurrentValue: "Non-semantic structure",
                    RuleType: "semanticStructure"
                ));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing adaptable content");
        }
    }

    private async Task<bool> HasSequenceIssue(IElementHandle element)
    {
        try
        {
            return await element.EvaluateAsync<bool>(@"el => {
                // Check for positioned elements that might break reading order
                const children = Array.from(el.children);
                const hasPositionedChildren = children.some(child => {
                    const style = window.getComputedStyle(child);
                    return style.position === 'absolute' || style.position === 'fixed';
                });

                if (!hasPositionedChildren) return false;

                // Check if visual order matches DOM order
                const positions = children.map(child => {
                    const rect = child.getBoundingClientRect();
                    return {
                        top: rect.top,
                        left: rect.left,
                        element: child
                    };
                });

                // Sort by visual position (top to bottom, left to right)
                const visualOrder = [...positions].sort((a, b) => {
                    const rowDiff = Math.abs(a.top - b.top);
                    if (rowDiff < 10) { // Same row
                        return a.left - b.left;
                    }
                    return a.top - b.top;
                });

                // Compare with DOM order
                return positions.some((pos, i) => pos.element !== visualOrder[i].element);
            }");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking for sequence issues");
            return false;
        }
    }

    private async Task<bool> HasOrientationRestriction(IPage page, IElementHandle element)
    {
        try
        {
            return await page.EvaluateAsync<bool>(@"(el) => {
                // Check for orientation-specific media queries in stylesheets
                const sheets = document.styleSheets;
                let hasOrientationRule = false;

                for (const sheet of sheets) {
                    try {
                        const rules = Array.from(sheet.cssRules || sheet.rules || []);
                        for (const rule of rules) {
                            if (rule instanceof CSSMediaRule) {
                                const mediaText = rule.conditionText || rule.media.mediaText;
                                if (mediaText.includes('orientation')) {
                                    // Check if any of the rules inside the media query affect our element
                                    const innerRules = Array.from(rule.cssRules);
                                    for (const innerRule of innerRules) {
                                        try {
                                            if (el.matches(innerRule.selectorText)) {
                                                hasOrientationRule = true;
                                                break;
                                            }
                                        } catch (e) {
                                            // Invalid selector, skip
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    } catch (e) {
                        // Skip cross-origin stylesheets
                        continue;
                    }
                }

                // Also check computed styles and inline styles
                const style = window.getComputedStyle(el);
                const hasRotateTransform = style.transform.includes('rotate');
                const hasOrientationStyles = 
                    el.style.cssText.includes('orientation') ||
                    hasRotateTransform ||
                    (style.display === 'none' && 
                     window.matchMedia('(orientation: portrait)').matches);

                return hasOrientationRule || hasOrientationStyles;
            }", element);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking for orientation restrictions");
            return false;
        }
    }

    // 1.4 Distinguishable (A)
    private async Task AnalyzeDistinguishableContent(IPage page, IElementHandle element, List<AccessibilityIssue> issues)
    {
        try
        {
            var styles = await element.EvaluateAsync<dynamic>(@"el => {
                const style = window.getComputedStyle(el);
                const bgImage = style.backgroundImage;
                
                // Helper to parse any CSS color format to RGB
                function parseColor(color) {
                    const div = document.createElement('div');
                    div.style.color = color;
                    document.body.appendChild(div);
                    const computed = window.getComputedStyle(div).color;
                    document.body.removeChild(div);
                    return computed.match(/\d+/g).map(Number);
                }
                
                // Get all computed styles
                return {
                    color: style.color,
                    backgroundColor: style.backgroundColor,
                    fontSize: parseFloat(style.fontSize),
                    lineHeight: style.lineHeight === 'normal' ? 1.2 : parseFloat(style.lineHeight),
                    hasBackgroundImage: bgImage !== 'none' && !bgImage.includes('gradient'),
                    isResizable: !el.style.cssText.includes('!important') &&
                               !style.maxWidth.includes('px') &&
                               style.overflow !== 'hidden',
                    // Parse colors using our helper
                    colorRGB: parseColor(style.color),
                    backgroundRGB: parseColor(style.backgroundColor)
                };
            }");

            // Check contrast ratio
            var colorRGB = ((object[])styles.colorRGB).Select(x => Convert.ToInt32(x)).ToArray();
            var backgroundRGB = ((object[])styles.backgroundRGB).Select(x => Convert.ToInt32(x)).ToArray();
            
            var contrastRatio = CalculateContrastRatio(
                colorRGB[0], colorRGB[1], colorRGB[2],
                backgroundRGB[0], backgroundRGB[1], backgroundRGB[2]
            );

            if (contrastRatio < 4.5) // WCAG AA standard for normal text
            {
                var elementHtml = await GetElementContext(element);
                issues.Add(new AccessibilityIssue(
                    Section: $"{SECTION}.4",
                    Description: "Text must have sufficient contrast with its background",
                    Element: elementHtml,
                    Impact: "High",
                    ScoreImpact: -10,
                    Suggestion: "Increase contrast ratio to at least 4.5:1",
                    CurrentValue: $"Contrast ratio {contrastRatio:F1}:1",
                    RuleType: "contrast"
                ));
            }

            // Check text over background images
            if (styles.hasBackgroundImage)
            {
                var hasOverlay = await element.EvaluateAsync<bool>(@"el => {
                    const style = window.getComputedStyle(el);
                    return style.position === 'relative' &&
                           Array.from(el.children).some(child => {
                               const childStyle = window.getComputedStyle(child);
                               return childStyle.backgroundColor !== 'rgba(0, 0, 0, 0)';
                           });
                }");

                if (!hasOverlay)
                {
                    var elementHtml = await GetElementContext(element);
                    issues.Add(new AccessibilityIssue(
                        Section: $"{SECTION}.4",
                        Description: "Text over images must be legible",
                        Element: elementHtml,
                        Impact: "High",
                        ScoreImpact: -10,
                        Suggestion: "Add a solid background or semi-transparent overlay behind text",
                        CurrentValue: "Text directly on image",
                        RuleType: "textOverImage"
                    ));
                }
            }

            // Check text resizing
            if (!styles.isResizable)
            {
                var elementHtml = await GetElementContext(element);
                issues.Add(new AccessibilityIssue(
                    Section: $"{SECTION}.4",
                    Description: "Text must be resizable without breaking layout",
                    Element: elementHtml,
                    Impact: "High",
                    ScoreImpact: -10,
                    Suggestion: "Remove fixed sizes and !important overrides",
                    CurrentValue: "Text not resizable",
                    RuleType: "textResize"
                ));
            }

            // Check font size (existing check)
            if (styles.fontSize < 16)
            {
                var elementHtml = await GetElementContext(element);
                issues.Add(new AccessibilityIssue(
                    Section: $"{SECTION}.4",
                    Description: "Text must be readable",
                    Element: elementHtml,
                    Impact: "High",
                    ScoreImpact: -10,
                    Suggestion: "Increase font size to at least 16px",
                    CurrentValue: $"{styles.fontSize}px",
                    RuleType: "fontSize"
                ));
            }

            // Check line height (existing check)
            if (styles.lineHeight < 1.5)
            {
                var elementHtml = await GetElementContext(element);
                issues.Add(new AccessibilityIssue(
                    Section: $"{SECTION}.4",
                    Description: "Text must have adequate line spacing",
                    Element: elementHtml,
                    Impact: "Medium",
                    ScoreImpact: -8,
                    Suggestion: "Increase line height to at least 1.5",
                    CurrentValue: $"{styles.lineHeight}",
                    RuleType: "lineHeight"
                ));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing distinguishable content");
        }
    }

    private double CalculateContrastRatio(int r1, int g1, int b1, int r2, int g2, int b2)
    {
        // Calculate relative luminance for both colors
        double l1 = GetRelativeLuminance(r1, g1, b1);
        double l2 = GetRelativeLuminance(r2, g2, b2);

        // Calculate contrast ratio
        double lighter = Math.Max(l1, l2);
        double darker = Math.Min(l1, l2);
        return (lighter + 0.05) / (darker + 0.05);
    }

    private double GetRelativeLuminance(int r, int g, int b)
    {
        // Convert to sRGB
        double rs = r / 255.0;
        double gs = g / 255.0;
        double bs = b / 255.0;

        // Convert to linear RGB
        double rl = rs <= 0.03928 ? rs / 12.92 : Math.Pow((rs + 0.055) / 1.055, 2.4);
        double gl = gs <= 0.03928 ? gs / 12.92 : Math.Pow((gs + 0.055) / 1.055, 2.4);
        double bl = bs <= 0.03928 ? bs / 12.92 : Math.Pow((bs + 0.055) / 1.055, 2.4);

        // Calculate luminance
        return 0.2126 * rl + 0.7152 * gl + 0.0722 * bl;
    }

    private async Task<string> GetElementContext(IElementHandle element)
    {
        try
        {
            return await element.EvaluateAsync<string>("el => el.outerHTML");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get element context");
            return string.Empty;
        }
    }
} 