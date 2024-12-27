using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Api.Accessibility.Models;

namespace Api.Accessibility.Services.Rules;

public class RuleEngine : IRuleEngine
{
    public bool ValidateRule(HtmlNode node, Rule rule)
    {
        return rule.Type switch
        {
            "fontSize" => ValidateFontSize(node, rule),
            "lineHeight" => ValidateLineHeight(node, rule),
            "contrast" => ValidateContrast(node, rule),
            "targetSize" => ValidateTargetSize(node, rule),
            "spacing" => ValidateSpacing(node, rule),
            "labelPresence" => ValidateLabelPresence(node),
            "ariaLabel" => ValidateAriaLabel(node),
            _ => true // Unknown rules pass by default
        };
    }

    public string GetSuggestionForRule(Rule rule, string elementHtml)
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

    private bool ValidateFontSize(HtmlNode node, Rule rule)
    {
        var style = node.GetAttributeValue("style", "");
        var fontSize = ExtractCssValue(style, "font-size");
        return fontSize >= rule.Min;
    }

    private bool ValidateLineHeight(HtmlNode node, Rule rule)
    {
        var style = node.GetAttributeValue("style", "");
        var lineHeight = ExtractCssValue(style, "line-height");
        return lineHeight >= rule.Min;
    }

    private bool ValidateContrast(HtmlNode node, Rule rule)
    {
        // In a real implementation, this would calculate actual contrast ratios
        // For now, we'll assume it passes
        return true;
    }

    private bool ValidateTargetSize(HtmlNode node, Rule rule)
    {
        var style = node.GetAttributeValue("style", "");
        var width = ExtractCssValue(style, "width");
        var height = ExtractCssValue(style, "height");
        return width >= rule.Min && height >= rule.Min;
    }

    private bool ValidateSpacing(HtmlNode node, Rule rule)
    {
        var style = node.GetAttributeValue("style", "");
        var margin = ExtractCssValue(style, "margin");
        var padding = ExtractCssValue(style, "padding");
        return margin >= rule.Min || padding >= rule.Min;
    }

    private bool ValidateLabelPresence(HtmlNode node)
    {
        if (node.Name != "input") return true;
        
        var id = node.GetAttributeValue("id", "");
        if (string.IsNullOrEmpty(id)) return false;

        var label = node.ParentNode?.SelectNodes($"//label[@for='{id}']");
        return label != null && label.Count > 0;
    }

    private bool ValidateAriaLabel(HtmlNode node)
    {
        return !string.IsNullOrEmpty(node.GetAttributeValue("aria-label", ""));
    }

    private static double ExtractCssValue(string style, string property)
    {
        var match = Regex.Match(style, $@"{property}:\s*(\d+\.?\d*)");
        return match.Success ? double.Parse(match.Groups[1].Value) : 0;
    }
} 