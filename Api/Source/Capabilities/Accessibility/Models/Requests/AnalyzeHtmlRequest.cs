using System.ComponentModel.DataAnnotations;

namespace Api.Accessibility.Models;

public record AnalyzeHtmlRequest(
    [Required] string Html
); 