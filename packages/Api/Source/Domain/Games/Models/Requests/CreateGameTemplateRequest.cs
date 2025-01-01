using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Games.Models.Requests;

public class CreateGameTemplateRequest
{
    [Required]
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsPublic { get; set; }

    public ICollection<CreateTemplateRoundRequest> Rounds { get; set; } = new List<CreateTemplateRoundRequest>();
}

public class CreateTemplateRoundRequest
{
    [Required]
    public required string Title { get; set; }
    
    public required RoundType Type { get; set; }
    
    public int TimeInMinutes { get; set; }
    
    public string? Instructions { get; set; }
    
    public int OrderIndex { get; set; }

    public ICollection<CreateTemplateRoundItemRequest> Items { get; set; } = new List<CreateTemplateRoundItemRequest>();
}

public class CreateTemplateRoundItemRequest
{
    [Required]
    public required string Title { get; set; }
    
    [Required]
    public required string Artist { get; set; }
    
    public int Points { get; set; }
    
    public string? ExtraInfo { get; set; }
    
    public string? SpotifyId { get; set; }
    
    public int OrderIndex { get; set; }
} 