using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Games.Models.Requests;

public class CreateRoundRequest
{
    [Required]
    public required string Title { get; set; }
    
    public required RoundType Type { get; set; }
    
    public int TimeInMinutes { get; set; }
    
    public string? Instructions { get; set; }
    
    public int OrderIndex { get; set; }
} 