using System.Text.Json;

namespace Api.Accessibility.Models;

public record CriteriaDefinition
{
    public required string Section { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Impact { get; init; }
    public required int ScoreImpact { get; init; }
    public required ValidationRules ValidationRules { get; init; }
}

public record ValidationRules
{
    public required string[] Selectors { get; init; }
    public required Rule[] Rules { get; init; }
}

public record Rule
{
    public required string Type { get; init; }
    public double Min { get; init; }
    public double Max { get; init; }
    public bool Required { get; init; }
}

public static class AccessibilityCriteria
{
    public static readonly IReadOnlyList<CriteriaDefinition> All = new List<CriteriaDefinition>
    {
        // Text Readability
        new()
        {
            Section = "EAA.1.1",
            Title = "Text Readability",
            Description = "Content must be easily readable for elderly users",
            Impact = "Critical",
            ScoreImpact = -15,
            ValidationRules = new ValidationRules
            {
                Selectors = new[] 
                { 
                    // Basic text elements
                    "p, h1, h2, h3, h4, h5, h6",
                    // Interactive elements
                    "button, a, input, textarea, select",
                    // Divs and spans that might contain text
                    "div:not([aria-hidden='true']), span:not([aria-hidden='true'])"
                },
                Rules = new[]
                {
                    new Rule { Type = "fontSize", Min = 16 },
                    new Rule { Type = "lineHeight", Min = 1.5 },
                    new Rule { Type = "contrast", Min = 7.0 }
                }
            }
        },

        // Touch Targets
        new()
        {
            Section = "EAA.2.1",
            Title = "Touch Target Size",
            Description = "Interactive elements must be large enough for users with motor impairments",
            Impact = "High",
            ScoreImpact = -10,
            ValidationRules = new ValidationRules
            {
                Selectors = new[] { "//button", "//a", "//*[@role='button']", "//input", "//select" },
                Rules = new[]
                {
                    new Rule { Type = "targetSize", Min = 44 },
                    new Rule { Type = "spacing", Min = 8 }
                }
            }
        },

        // Input Assistance
        new()
        {
            Section = "EAA.8.1",
            Title = "Input Assistance",
            Description = "Help users avoid and correct mistakes",
            Impact = "Critical",
            ScoreImpact = -12,
            ValidationRules = new ValidationRules
            {
                Selectors = new[] { "//input", "//form", "//select", "//textarea" },
                Rules = new[]
                {
                    new Rule { Type = "labelPresence", Required = true },
                    new Rule { Type = "ariaLabel", Required = true }
                }
            }
        }
    };
} 