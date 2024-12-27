using Api.Accessibility.Models;
namespace Api.Accessibility.Services;

public interface IAccessibilityAnalyzer
{
    Task<AnalysisResult> AnalyzeUrlAsync(string url);
} 