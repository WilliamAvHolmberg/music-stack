using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Api.Study;

namespace Api.Flashcards;

public class Flashcard
{
    public int Id { get; set; }
    public required string Question { get; set; }
    public required string Answer { get; set; }
    public int Importance { get; set; }
    public DateTime CreatedAt { get; set; }
    public int FlashcardSetId { get; set; }
    
    [JsonIgnore]
    public FlashcardSet? FlashcardSet { get; set; }
    
    public int? ConceptId { get; set; }
    
    [JsonIgnore]
    public Concept? Concept { get; set; }
    
    public List<FlashcardReview> ReviewHistory { get; set; } = new();
    public string? Notes { get; set; }
    public bool IsMarkedForReview { get; set; }
    
    // Review tracking
    public int ReviewCount { get; set; }
    public int SuccessCount { get; set; }
    public DateTime LastReviewDate { get; set; } = DateTime.UtcNow;
    public DateTime NextReviewDate { get; set; } = DateTime.UtcNow;
} 