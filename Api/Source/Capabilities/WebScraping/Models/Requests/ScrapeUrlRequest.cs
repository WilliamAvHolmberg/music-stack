using System.ComponentModel.DataAnnotations;

namespace Api.WebScraping.Models.Requests;

public record ScrapeUrlRequest(
    [Required] string Url
); 