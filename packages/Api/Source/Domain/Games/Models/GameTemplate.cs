using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Games.Models;

public class GameTemplate
{
    public int Id { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    
    public bool IsPublic { get; set; }
    
    // Navigation properties
    public ICollection<Round> Rounds { get; set; } = new List<Round>();
    public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
} 