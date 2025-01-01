using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Games.Models.Requests;

public class CreateTeamRequest
{
    [Required]
    public required string Name { get; set; }
    
    [Required]
    public required string Color { get; set; }
} 