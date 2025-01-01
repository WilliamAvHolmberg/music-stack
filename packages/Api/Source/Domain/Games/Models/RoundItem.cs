using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Games.Models;

public class RoundItem
{
    public int Id { get; set; }
    
    public int RoundId { get; set; }
    
    public int OrderIndex { get; set; }
    
    [Required]
    public required string Title { get; set; }
    
    [Required]
    public required string Artist { get; set; }
    
    public int Points { get; set; }
    
    public string? ExtraInfo { get; set; }
    
    public bool IsAnswerRevealed { get; set; }
    
    public string? SpotifyId { get; set; }
    
    public int Year { get; set; }
    
    // Navigation property
    public Round? Round { get; set; }
} 