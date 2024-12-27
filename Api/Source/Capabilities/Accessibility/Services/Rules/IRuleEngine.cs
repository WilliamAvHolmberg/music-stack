using HtmlAgilityPack;
using Api.Accessibility.Models;

namespace Api.Accessibility.Services.Rules;

public interface IRuleEngine
{
    bool ValidateRule(HtmlNode node, Rule rule);
    string GetSuggestionForRule(Rule rule, string elementHtml);
} 