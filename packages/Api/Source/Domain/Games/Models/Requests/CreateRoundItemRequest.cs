using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Games.Models.Requests;

public class CreateRoundItemRequest
{
    [Required]
    public required string Title { get; set; }
    
    [Required]
    public required string Artist { get; set; }
    
    public int Points { get; set; }
    
    public string? ExtraInfo { get; set; }
    
    public int OrderIndex { get; set; }

    public string? SpotifyId { get; set; }
} 