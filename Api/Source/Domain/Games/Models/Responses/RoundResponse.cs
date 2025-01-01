using Api.Domain.Songs.Models.Responses;

namespace Api.Domain.Games.Models.Responses;

public class RoundResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required RoundType Type { get; set; }
    public required RoundStatus Status { get; set; }
    public required int TimeInMinutes { get; set; }
    public string? Instructions { get; set; }
    public required int OrderIndex { get; set; }
    public required ICollection<RoundItemResponse> Items { get; set; }
    public int TimeLeft { get; set; }
    public bool IsPaused { get; set; }
    public bool IsAnswerRevealed { get; set; }
} 