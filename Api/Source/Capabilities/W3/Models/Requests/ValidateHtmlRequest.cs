using System.ComponentModel.DataAnnotations;

namespace Api.W3.Models.Requests;

public record ValidateHtmlRequest(
    [Required] string Html
); 