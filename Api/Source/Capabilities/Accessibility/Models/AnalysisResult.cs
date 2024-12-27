namespace Api.Accessibility.Models;

public record AccessibilitySection(
    string Section,
    string Title,
    string Description,
    string Impact,
    int TotalIssues,
    double ScoreImpact,
    List<AccessibilityIssueGroup> IssueGroups
);

public record AccessibilityIssueGroup(
    string RuleType,
    int Count,
    string Suggestion,
    List<string> Examples,
    List<string> CurrentValues
);

public record AnalysisResult(
    List<AccessibilitySection> Sections,
    List<AccessibilityIssue> DetailedIssues,
    double Score
);

public record AccessibilityIssue(
    string Section,
    string Description,
    string Element,
    string Impact,
    int ScoreImpact,
    string Suggestion,
    string CurrentValue,
    string RuleType
);