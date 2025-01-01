using Api.Domain.Songs.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Games.Models;

public enum RoundType
{
    GuessTheMelody,
    FirstLine,
    ArtistQuiz
}

public enum RoundStatus
{
    NotStarted,
    InProgress,
    Completed
}

public class Round
{
    public int Id { get; set; }
    
    public int? GameSessionId { get; set; }
    public int? GameTemplateId { get; set; }
    
    public required RoundType Type { get; set; }
    
    public required RoundStatus Status { get; set; }
    
    [Required]
    public required string Title { get; set; }
    
    public int OrderIndex { get; set; }
    
    public int TimeInMinutes { get; set; }
    
    public string? Instructions { get; set; }
    
    public int? CurrentSongId { get; set; }
    
    public int TimeLeft { get; set; }
    
    public bool IsPaused { get; set; }
    
    public bool IsAnswerRevealed { get; set; }
    
    // Navigation properties
    public GameSession? GameSession { get; set; }
    public GameTemplate? GameTemplate { get; set; }
    public Song? CurrentSong { get; set; }
    public ICollection<RoundItem> Items { get; set; } = new List<RoundItem>();
} 