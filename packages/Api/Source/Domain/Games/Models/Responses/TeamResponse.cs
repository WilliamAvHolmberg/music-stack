namespace Api.Domain.Games.Models.Responses;

public class TeamResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int Score { get; set; }
    public required string Color { get; set; }
} 