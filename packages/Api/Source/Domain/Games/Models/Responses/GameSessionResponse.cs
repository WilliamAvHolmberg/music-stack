namespace Api.Domain.Games.Models.Responses;

public class GameSessionResponse
{
    public required int Id { get; set; }
    public required GameStatus Status { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required int CurrentRoundIndex { get; set; }
    public required int CurrentItemIndex { get; set; }
    public required ICollection<TeamResponse> Teams { get; set; }
    public required ICollection<RoundResponse> Rounds { get; set; }
} 