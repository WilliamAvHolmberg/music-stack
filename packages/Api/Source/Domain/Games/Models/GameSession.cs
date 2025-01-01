using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Games.Models;

public enum GameStatus
{
    NotStarted,
    InProgress,
    Finished
}

public class GameSession
{
    public int Id { get; set; }
    
    public int? GameTemplateId { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    
    public required GameStatus Status { get; set; }
    
    public int CurrentRoundIndex { get; set; }
    
    public int CurrentItemIndex { get; set; }
    
    // Navigation properties
    public GameTemplate? GameTemplate { get; set; }
    public ICollection<Team> Teams { get; set; } = new List<Team>();
    public ICollection<Round> Rounds { get; set; } = new List<Round>();
} 