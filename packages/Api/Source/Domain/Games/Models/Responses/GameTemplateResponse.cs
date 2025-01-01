namespace Api.Domain.Games.Models.Responses;

public class GameTemplateResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required bool IsPublic { get; set; }
    public required ICollection<RoundResponse> Rounds { get; set; }
} 