namespace Api.WebScraping.Models.Responses;

public record ScrapeUrlResponse(
    string Html,
    string Screenshot,
    double LoadTimeSeconds
); 