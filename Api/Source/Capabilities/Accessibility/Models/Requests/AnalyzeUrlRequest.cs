using System.ComponentModel.DataAnnotations;

namespace Api.Accessibility.Models;

public record AnalyzeUrlRequest(
    [Required] string Url
); 