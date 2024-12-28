using Microsoft.Playwright;
using Api.Accessibility.Models;

namespace Api.Accessibility.Services;

public interface ISection1Service
{
    Task<List<AccessibilityIssue>> AnalyzeAsync(IPage page, IElementHandle element);
} 