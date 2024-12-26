using System.Text.Json.Serialization;

namespace Api.Flashcards;

public enum ReviewStage
{
    Initial = 0,
    TenMinutes = 1,
    OneHour = 2,
    OneDay = 3,
    OneWeek = 4,
    OneMonth = 5,
    ThreeMonths = 6
}

public class ReviewSchedule
{
    public int Id { get; set; }
    public int FlashcardId { get; set; }
    public DateTime LastReviewedAt { get; set; }
    public DateTime NextReviewAt { get; set; }
    public int CurrentInterval { get; set; }  // in minutes
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public float ConfidenceLevel { get; set; }  // 0-1
    public ReviewStage Stage { get; set; }
    
    [JsonIgnore]
    public Flashcard Flashcard { get; set; } = null!;
} 