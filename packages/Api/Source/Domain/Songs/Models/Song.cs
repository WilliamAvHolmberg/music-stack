using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Songs.Models;

public enum SongCategory
{
    Pop,
    Rock,
    Schlager
}

public enum SongLanguage
{
    Swedish,
    English
}

public class Song
{
    public int Id { get; set; }
    
    [Required]
    public required string Title { get; set; }
    
    [Required]
    public required string Artist { get; set; }
    
    [Required]
    public required string FirstLine { get; set; }
    
    public int? Year { get; set; }
    
    [Range(1, 3)]
    public required int Difficulty { get; set; }
    
    public required SongCategory Category { get; set; }
    
    public required SongLanguage Language { get; set; }
    
    public string? SpotifyId { get; set; }
} 