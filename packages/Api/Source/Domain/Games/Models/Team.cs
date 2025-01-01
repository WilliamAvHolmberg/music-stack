using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Games.Models;

public class Team
{
    public int Id { get; set; }
    
    public int GameSessionId { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    public int Score { get; set; }
    
    [Required]
    public required string Color { get; set; }
    
    // Navigation property
    public GameSession? GameSession { get; set; }
} 