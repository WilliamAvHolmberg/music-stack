using System.ComponentModel.DataAnnotations;

namespace Api.Flashcards;

public class GenerateFlashcardsRequest
{
    [Required]
    public required string Title { get; set; }
    
    [Required]
    public required string Content { get; set; }
    
    public string Model { get; set; } = "gpt-4";
    public string Provider { get; set; } = "openai";
} 