namespace Api.Domain.Games.Models.Responses;

public class RoundItemResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Artist { get; set; }
    public required int Points { get; set; }
    public string? ExtraInfo { get; set; }
    public required int OrderIndex { get; set; }
    public bool IsAnswerRevealed { get; set; }
    public string? SpotifyId { get; set; }
    public required int Year { get; set; }
} 