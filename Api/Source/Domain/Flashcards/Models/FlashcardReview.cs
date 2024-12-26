using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Flashcards;

public class FlashcardReview
{
    public int Id { get; set; }
    public int FlashcardId { get; set; }
    [JsonIgnore]
    public Flashcard Flashcard { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public DateTime ReviewedAt { get; set; }
    public string? Comment { get; set; }
} 