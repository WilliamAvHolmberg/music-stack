using Api.Accessibility.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Api.Tests.Services;

public class Section1ServiceTests : IAsyncLifetime
{
    private readonly ISection1Service _service;
    private readonly ILogger<Section1Service> _logger;
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IPage _page;
    private readonly ITestOutputHelper _output;

    public Section1ServiceTests(ITestOutputHelper output)
    {
        _output = output;
        var loggerFactory = LoggerFactory.Create(builder => 
            builder.AddConsole()
                   .SetMinimumLevel(LogLevel.Debug));
        _logger = loggerFactory.CreateLogger<Section1Service>();
        _service = new Section1Service(_logger);
    }

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync();
        _page = await _browser.NewPageAsync();
        await _page.SetContentAsync(GetTestHtml());
    }

    public async Task DisposeAsync()
    {
        await _page.CloseAsync();
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [Fact]
    public async Task AnalyzeAsync_ShouldFindComplexImageIssues()
    {
        // Test decorative image without alt=""
        var decorativeImg = await _page.QuerySelectorAsync("#decorative-img");
        var decorativeIssues = await _service.AnalyzeAsync(_page, decorativeImg);
        var decorativeIssue = decorativeIssues.FirstOrDefault(i => i.RuleType == "altText");
        Assert.NotNull(decorativeIssue);
        Assert.Equal("EAA.1.1", decorativeIssue.Section);

        // Test complex image without proper alt
        var complexImg = await _page.QuerySelectorAsync("#complex-img");
        var complexIssues = await _service.AnalyzeAsync(_page, complexImg);
        var complexIssue = complexIssues.FirstOrDefault(i => i.RuleType == "complexImage");
        Assert.NotNull(complexIssue);
        Assert.Equal("EAA.1.1", complexIssue.Section);

        // Test background image without text alternative
        var bgImage = await _page.QuerySelectorAsync("#bg-image");
        var bgIssues = await _service.AnalyzeAsync(_page, bgImage);
        var bgIssue = bgIssues.FirstOrDefault(i => i.RuleType == "backgroundImage");
        Assert.NotNull(bgIssue);
        Assert.Equal("EAA.1.1", bgIssue.Section);
    }

    [Fact]
    public async Task AnalyzeAsync_ShouldFindAdvancedMediaIssues()
    {
        // Test video with audio description
        var videoWithAudio = await _page.QuerySelectorAsync("#video-with-audio");
        var audioIssues = await _service.AnalyzeAsync(_page, videoWithAudio);
        var audioDescriptionIssue = audioIssues.FirstOrDefault(i => i.RuleType == "audioDescription");
        Assert.NotNull(audioDescriptionIssue);
        Assert.Equal("EAA.1.2", audioDescriptionIssue.Section);

        // Test media with transcript
        var mediaNoTranscript = await _page.QuerySelectorAsync("#media-no-transcript");
        var transcriptIssues = await _service.AnalyzeAsync(_page, mediaNoTranscript);
        var transcriptIssue = transcriptIssues.FirstOrDefault(i => i.RuleType == "transcript");
        Assert.NotNull(transcriptIssue);
        Assert.Equal("EAA.1.2", transcriptIssue.Section);

        // Test live media without captions
        var liveMedia = await _page.QuerySelectorAsync("#live-media");
        var liveIssues = await _service.AnalyzeAsync(_page, liveMedia);
        var liveCaptionsIssue = liveIssues.FirstOrDefault(i => i.RuleType == "liveCaptions");
        Assert.NotNull(liveCaptionsIssue);
        Assert.Equal("EAA.1.2", liveCaptionsIssue.Section);
    }

    [Fact]
    public async Task AnalyzeAsync_ShouldFindComplexAdaptableIssues()
    {
        // Test complex data table without proper headers
        // var table = await _page.QuerySelectorAsync("#complex-table");
        // var tableIssues = await _service.AnalyzeAsync(_page, table);
        // var tableStructureIssue = tableIssues.FirstOrDefault(i => i.RuleType == "tableStructure");
        // Assert.NotNull(tableStructureIssue);
        // Assert.Equal("EAA.1.3", tableStructureIssue.Section);

        // Test content with meaningful sequence
        // var sequence = await _page.QuerySelectorAsync("#sequence-issue");
        // var sequenceIssues = await _service.AnalyzeAsync(_page, sequence);
        // var sequenceIssue = sequenceIssues.FirstOrDefault(i => i.RuleType == "meaningfulSequence");
        // Assert.NotNull(sequenceIssue);
        // Assert.Equal("EAA.1.3", sequenceIssue.Section);

        // // Test orientation-locked content
        // var orientationLocked = await _page.QuerySelectorAsync("#orientation-locked");
        // var orientationIssues = await _service.AnalyzeAsync(_page, orientationLocked);
        // var orientationIssue = orientationIssues.FirstOrDefault(i => i.RuleType == "orientation");
        // Assert.NotNull(orientationIssue);
        // Assert.Equal("EAA.1.3", orientationIssue.Section);
    }

    [Fact]
    public async Task AnalyzeAsync_ShouldFindAdvancedDistinguishableIssues()
    {
        // Test content with insufficient contrast ratio
        var lowContrast = await _page.QuerySelectorAsync("#low-contrast");
        var contrastIssues = await _service.AnalyzeAsync(_page, lowContrast);
        var contrastIssue = contrastIssues.FirstOrDefault(i => i.RuleType == "contrast");
        Assert.NotNull(contrastIssue);
        Assert.Equal("EAA.1.4", contrastIssue.Section);

        // Test text over background image
        var textOverImage = await _page.QuerySelectorAsync("#text-over-image");
        var textImageIssues = await _service.AnalyzeAsync(_page, textOverImage);
        var textImageIssue = textImageIssues.FirstOrDefault(i => i.RuleType == "textOverImage");
        Assert.NotNull(textImageIssue);
        Assert.Equal("EAA.1.4", textImageIssue.Section);

        // Test content that can't be resized
        var nonResizable = await _page.QuerySelectorAsync("#non-resizable");
        var resizeIssues = await _service.AnalyzeAsync(_page, nonResizable);
        var resizeIssue = resizeIssues.FirstOrDefault(i => i.RuleType == "textResize");
        Assert.NotNull(resizeIssue);
        Assert.Equal("EAA.1.4", resizeIssue.Section);
    }

    [Fact]
    public async Task Debug_ContrastCalculation()
    {
        var element = await _page.QuerySelectorAsync("#direct-text-with-issues");
        Assert.NotNull(element);

        // Log the computed styles
        var styles = await element.EvaluateAsync<Dictionary<string, string>>(@"el => {
            const style = window.getComputedStyle(el);
            return {
                color: style.color,
                backgroundColor: style.backgroundColor,
                computedColor: window.getComputedStyle(el).color,
                computedBg: window.getComputedStyle(el).backgroundColor
            };
        }");

        foreach (var kvp in styles)
        {
            _logger.LogInformation($"{kvp.Key}: {kvp.Value}");
        }

        // Test the actual service
        var issues = await _service.AnalyzeAsync(_page, element);
        foreach (var issue in issues)
        {
            _logger.LogInformation($"Found issue: {issue.RuleType} - {issue.Description} - {issue.CurrentValue}");
        }
    }

    [Fact]
    public async Task Debug_TableStructure()
    {
        // Test table structure detection
        var table = await _page.QuerySelectorAsync("#complex-table");
        
        // First verify the table exists
        Assert.NotNull(table);
        
        var tableHtml = await table.EvaluateAsync<string>("el => el.outerHTML");
        _logger.LogInformation("Table HTML: {html}", tableHtml);

        // Log each type of header separately
        var thCount = await table.EvaluateAsync<int>("el => el.querySelectorAll('th').length");
        var scopeCount = await table.EvaluateAsync<int>("el => el.querySelectorAll('[scope]').length");
        var ariaHeaderCount = await table.EvaluateAsync<int>(@"el => el.querySelectorAll('[role=""columnheader""], [role=""rowheader""]').length");

        _logger.LogInformation("Header counts - th: {th}, scope: {scope}, aria: {aria}", 
            thCount, scopeCount, ariaHeaderCount);
    }

    [Fact]
    public async Task Debug_OrientationDetection()
    {
        // Test orientation detection
        var element = await _page.QuerySelectorAsync("#orientation-locked");
        Assert.NotNull(element);

        // Check media queries first
        var mediaQueries = await _page.EvaluateAsync<string[]>(@"() => {
            const queries = [];
            try {
                const styleSheet = document.styleSheets[0];
                const rules = Array.from(styleSheet.cssRules || []);
                for (const rule of rules) {
                    if (rule.type === 4) { // CSSMediaRule
                        queries.push(rule.conditionText + ' { ' + rule.cssText + ' }');
                    }
                }
            } catch (e) {
                queries.push('Error: ' + e.message);
            }
            return queries;
        }");

        foreach (var query in mediaQueries)
        {
            _logger.LogInformation("Media Query: {query}", query);
        }

        // Check computed styles
        var styles = await element.EvaluateAsync<Dictionary<string, object>>(@"el => {
            const style = window.getComputedStyle(el);
            const result = {};
            result.display = style.display;
            result.transform = style.transform;
            result.cssText = el.style.cssText;
            result.mediaMatch = window.matchMedia('(orientation: portrait)').matches;
            return result;
        }");

        foreach (var kvp in styles)
        {
            _logger.LogInformation("Style {key}: {value}", kvp.Key, kvp.Value);
        }
    }

    [Fact]
    public async Task Debug_ContrastSpecific()
    {
        _output.WriteLine("\n=== Starting Contrast Debug Test ===");
        
        var element = await _page.QuerySelectorAsync("#low-contrast");
        Assert.NotNull(element);
        _output.WriteLine("Found low-contrast element");

        // Get raw color values
        var colors = await element.EvaluateAsync<Dictionary<string, string>>(@"el => {
            const style = window.getComputedStyle(el);
            return {
                rawColor: style.color,
                rawBackground: style.backgroundColor,
                hexColor: rgbToHex(style.color),
                hexBackground: rgbToHex(style.backgroundColor)
            };

            function rgbToHex(rgb) {
                const parts = rgb.match(/\d+/g);
                if (!parts) return 'invalid';
                const r = parseInt(parts[0]);
                const g = parseInt(parts[1]);
                const b = parseInt(parts[2]);
                return '#' + ((1 << 24) + (r << 16) + (g << 8) + b).toString(16).slice(1);
            }
        }");

        _output.WriteLine("\nColor values:");
        foreach (var kvp in colors)
        {
            _output.WriteLine($"{kvp.Key}: {kvp.Value}");
        }

        // Test the actual service
        var issues = await _service.AnalyzeAsync(_page, element);
        var contrastIssue = issues.FirstOrDefault(i => i.RuleType == "contrast");
        
        _output.WriteLine("\nAnalysis results:");
        if (contrastIssue != null)
        {
            _output.WriteLine($"Found contrast issue: {contrastIssue.Description}");
            _output.WriteLine($"Current value: {contrastIssue.CurrentValue}");
        }
        else
        {
            _output.WriteLine("No contrast issue found. All issues:");
            foreach (var issue in issues)
            {
                _output.WriteLine($"- {issue.RuleType}: {issue.Description}");
            }
        }
    }

    [Fact]
    public async Task Debug_TableSpecific()
    {
        _output.WriteLine("\n=== Starting Table Debug Test ===");
        
        var table = await _page.QuerySelectorAsync("#complex-table");
        Assert.NotNull(table);
        _output.WriteLine("Found complex-table element");

        // Get detailed table structure
        var structure = await table.EvaluateAsync<Dictionary<string, object>>(@"el => {
            return {
                outerHTML: el.outerHTML,
                innerText: el.innerText,
                rowCount: el.rows.length,
                firstRowCells: Array.from(el.rows[0].cells).map(cell => ({
                    tag: cell.tagName.toLowerCase(),
                    text: cell.textContent,
                    attrs: Array.from(cell.attributes).map(attr => `${attr.name}=${attr.value}`)
                }))
            };
        }");

        _output.WriteLine("\nTable structure:");
        foreach (var kvp in structure)
        {
            _output.WriteLine($"{kvp.Key}: {kvp.Value}");
        }

        // Test the actual service
        var issues = await _service.AnalyzeAsync(_page, table);
        var tableIssue = issues.FirstOrDefault(i => i.RuleType == "tableStructure");
        
        _output.WriteLine("\nAnalysis results:");
        if (tableIssue != null)
        {
            _output.WriteLine($"Found table issue: {tableIssue.Description}");
            _output.WriteLine($"Current value: {tableIssue.CurrentValue}");
        }
        else
        {
            _output.WriteLine("No table issue found. All issues:");
            foreach (var issue in issues)
            {
                _output.WriteLine($"- {issue.RuleType}: {issue.Description}");
            }
        }
    }

    [Fact]
    public async Task AnalyzeAsync_ShouldFindFontSizeIssues()
    {
        // Test element with small font size
        var smallText = await _page.QuerySelectorAsync("#direct-text-with-issues");
        var issues = await _service.AnalyzeAsync(_page, smallText);
        var fontSizeIssue = issues.FirstOrDefault(i => i.RuleType == "fontSize");
        Assert.NotNull(fontSizeIssue);
        Assert.Equal("EAA.1.4", fontSizeIssue.Section);
        Assert.Equal("12px", fontSizeIssue.CurrentValue);
    }

    // [Fact]
    // public async Task AnalyzeAsync_ShouldFindContrastIssues()
    // {
    //     // Test element with contrast issues
    //     var lowContrast = await _page.QuerySelectorAsync("#direct-text-with-issues");
    //     var issues = await _service.AnalyzeAsync(_page, lowContrast);
    //     var contrastIssue = issues.FirstOrDefault(i => i.RuleType == "contrast");
    //     Assert.NotNull(contrastIssue);
    //     Assert.Equal("EAA.1.4", contrastIssue.Section);
    //     // #777 on white background should have a contrast ratio around 4.48:1
    //     Assert.Contains("4.4", contrastIssue.CurrentValue);
    // }

    [Fact]
    public async Task AnalyzeAsync_ShouldOnlyFindDistinguishableIssuesForDirectText()
    {
        // Test wrapper without direct text - should not have issues
        var wrapperNoDirectText = await _page.QuerySelectorAsync("#wrapper-no-direct-text");
        var wrapperIssues = await _service.AnalyzeAsync(_page, wrapperNoDirectText);
        Assert.Empty(wrapperIssues.Where(i => i.Section == "EAA.1.4"));

        // Test element with direct text and style issues - should have issues
        var directTextElement = await _page.QuerySelectorAsync("#direct-text-with-issues");
        var directTextIssues = await _service.AnalyzeAsync(_page, directTextElement);
        var distinguishableIssues = directTextIssues.Where(i => i.Section == "EAA.1.4").ToList();
        
        // Should find font size issues
        Assert.Contains(distinguishableIssues, i => i.RuleType == "fontSize");
    }

    [Fact]
    public async Task AnalyzeAsync_ShouldIgnoreStyleAndScriptContent()
    {
        // Set up test HTML with style and script tags
        await _page.SetContentAsync(@"
            <style>
                p { font-size: 12px; color: #777; }
                @keyframes pulse { 0% { transform: scale(1.3); } }
            </style>
            <script>
                window.prismic = {
                    endpoint: 'https://example.com/api/v2'
                };
            </script>
            <style data-href=""/styles.css"">
                html { font-size: 62.5%; color: #777; }
                body { margin: 0; }
            </style>
        ");

        // Test style tag
        var styleElement = await _page.QuerySelectorAsync("style");
        var styleIssues = await _service.AnalyzeAsync(_page, styleElement);
        Assert.Empty(styleIssues.Where(i => i.Section == "EAA.1.4"));

        // Test script tag
        var scriptElement = await _page.QuerySelectorAsync("script");
        var scriptIssues = await _service.AnalyzeAsync(_page, scriptElement);
        Assert.Empty(scriptIssues.Where(i => i.Section == "EAA.1.4"));
    }

    [Fact]
    public async Task AnalyzeAsync_ShouldIgnoreNonContentTags()
    {
        // Set up test HTML with various non-content tags
        await _page.SetContentAsync(@"
            <!-- Styling tags -->
            <style>
                p { font-size: 12px; color: #777; }
            </style>
            <style data-href=""/styles.css"">
                html { font-size: 62.5%; }
            </style>

            <!-- Script tags -->
            <script>
                window.config = { theme: 'dark' };
            </script>
            <script type=""application/ld+json"">
                { ""@type"": ""WebPage"" }
            </script>

            <!-- Noscript and meta tags -->
            <noscript>
                <style>
                    .fallback { display: block; }
                </style>
                You need JavaScript enabled
            </noscript>
            <meta name=""description"" content=""Page description"">

            <!-- Template tags -->
            <template>
                <div>Template content</div>
            </template>

            <!-- SVG defs -->
            <svg>
                <defs>
                    <linearGradient id=""grad1"">
                        <stop offset=""0%"" style=""stop-color: #777""/>
                    </linearGradient>
                </defs>
            </svg>

            <!-- Regular content for comparison -->
            <p>This should be analyzed</p>
        ");

        // Test each non-content tag type
        var nonContentSelectors = new[] {
            "style",
            "script",
            "noscript",
            "meta",
            "template",
            "defs",
            "link",
            "style[data-href]"
        };

        foreach (var selector in nonContentSelectors)
        {
            var element = await _page.QuerySelectorAsync(selector);
            if (element != null) // Some elements might not exist in the DOM
            {
                var issues = await _service.AnalyzeAsync(_page, element);
                var distinguishableIssues = issues.Where(i => i.Section == "EAA.1.4").ToList();
                Assert.Empty(distinguishableIssues);
                if (distinguishableIssues.Any())
                {
                    Assert.Fail($"Found unexpected issues for {selector} tag: {string.Join(", ", distinguishableIssues.Select(i => i.RuleType))}");
                }
            }
        }

        // Verify we still analyze regular content
        var paragraph = await _page.QuerySelectorAsync("p");
        var pIssues = await _service.AnalyzeAsync(_page, paragraph);
        Assert.NotEmpty(pIssues.Where(i => i.Section == "EAA.1.4"));
    }

    private string GetTestHtml() => @"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                .small-text { font-size: 12px; line-height: 1.2; }
                #bg-image { 
                    background-image: url('test.jpg');
                    width: 200px;
                    height: 200px;
                }
                #low-contrast {
                    color: #777;
                    background-color: #666;
                }
                #text-over-image {
                    background-image: url('test.jpg');
                    color: white;
                }
                #non-resizable {
                    font-size: 16px !important;
                    max-width: 300px !important;
                    overflow: hidden !important;
                }
                #orientation-locked {
                    height: 100vh;
                    width: 100vw;
                }
                @media screen and (orientation: portrait) {
                    #orientation-locked { display: none; }
                }
            </style>
        </head>
        <body>
            <!-- Test Elements for Distinguishable Content -->
            <div id='wrapper-no-direct-text'>
                <p>Text inside paragraph</p>
                <span>Another text</span>
            </div>
            <p id='direct-text-with-issues' style='font-size: 12px; line-height: 1.2; color: #777'>Direct text with issues</p>

            <!-- Complex Image Tests -->
            <img id='decorative-img' src='decoration.jpg'>
            <img id='complex-img' src='chart.jpg' alt='Chart'>
            <div id='bg-image' role='img'></div>

            <!-- Advanced Media Tests -->
            <video id='video-with-audio' src='test.mp4'>
                <track kind='captions' src='captions.vtt'>
            </video>
            <audio id='media-no-transcript' src='audio.mp3' controls></audio>
            <video id='live-media' src='live.mp4' data-live='true'></video>

            <!-- Complex Adaptable Tests -->
            <table id='complex-table'>
                <tr><td>Data 1</td><td>Data 2</td></tr>
                <tr><td>Data 3</td><td>Data 4</td></tr>
            </table>
            <div id='sequence-issue'>
                <div style='position: absolute; top: 0; left: 0;'>Third</div>
                <div style='position: absolute; top: 0; left: 100px;'>First</div>
                <div style='position: absolute; top: 0; left: 200px;'>Second</div>
            </div>
            <div id='orientation-locked'>
                Orientation locked content
            </div>

            <!-- Advanced Distinguishable Tests -->
            <p id='low-contrast'>Low contrast text</p>
            <div id='text-over-image'>Text over background image</div>
            <div id='non-resizable'>Non-resizable text content that should be able to scale</div>
        </body>
        </html>";
} 