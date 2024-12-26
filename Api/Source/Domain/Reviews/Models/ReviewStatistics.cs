namespace Api.Reviews;

public class ReviewStatistics
{
    public int TotalFlashcards { get; set; }
    public int DueFlashcards { get; set; }
    public int CompletedReviews { get; set; }
    public double AverageScore { get; set; }
    public int StreakDays { get; set; }
    public DateTime LastReviewDate { get; set; }
    public DateTime NextDueDate { get; set; }
    public int MasteredFlashcards { get; set; }  // High success rate
    public int StrugglingFlashcards { get; set; } // Low success rate
    public Dictionary<string, int> ReviewsByDay { get; set; } = new();
} 