using Api.Infrastructure;
using Api.Flashcards;
using Microsoft.EntityFrameworkCore;

namespace Api.Reviews;

public interface IReviewService
{
    Task<IEnumerable<Flashcard>> GetFlashcardsForStudyAsync(int flashcardSetId);
    Task<ReviewStatistics> ProcessAnswerAsync(int flashcardId, bool isCorrect);
    Task<ReviewStatistics> GetStudyStatisticsAsync(int flashcardSetId);
}

public class ReviewService : IReviewService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ReviewService> _logger;

    public ReviewService(AppDbContext context, ILogger<ReviewService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Flashcard>> GetFlashcardsForStudyAsync(int flashcardSetId)
    {
        var now = DateTime.UtcNow;
        return await _context.Flashcards
            .Where(f => f.FlashcardSetId == flashcardSetId && f.NextReviewDate <= now)
            .OrderBy(f => f.LastReviewDate)
            .ToListAsync();
    }

    public async Task<ReviewStatistics> ProcessAnswerAsync(int flashcardId, bool isCorrect)
    {
        var flashcard = await _context.Flashcards
            .Include(f => f.FlashcardSet)
            .FirstOrDefaultAsync(f => f.Id == flashcardId)
            ?? throw new Exception("Flashcard not found");

        flashcard.LastReviewDate = DateTime.UtcNow;
        flashcard.NextReviewDate = CalculateNextReviewDate(flashcard, isCorrect);
        flashcard.ReviewCount++;
        flashcard.SuccessCount += isCorrect ? 1 : 0;

        await _context.SaveChangesAsync();

        return await GetStudyStatisticsAsync(flashcard.FlashcardSetId);
    }

    public async Task<ReviewStatistics> GetStudyStatisticsAsync(int flashcardSetId)
    {
        var flashcards = await _context.Flashcards
            .Where(f => f.FlashcardSetId == flashcardSetId)
            .ToListAsync();

        var now = DateTime.UtcNow;
        var stats = new ReviewStatistics
        {
            TotalFlashcards = flashcards.Count,
            DueFlashcards = flashcards.Count(f => f.NextReviewDate <= now),
            CompletedReviews = flashcards.Sum(f => f.ReviewCount),
            AverageScore = flashcards.Any(f => f.ReviewCount > 0)
                ? flashcards.Where(f => f.ReviewCount > 0)
                    .Average(f => (double)f.SuccessCount / f.ReviewCount)
                : 0,
            LastReviewDate = flashcards.Max(f => f.LastReviewDate),
            NextDueDate = flashcards.Min(f => f.NextReviewDate),
            MasteredFlashcards = flashcards.Count(f => f.ReviewCount > 0 && (double)f.SuccessCount / f.ReviewCount > 0.8),
            StrugglingFlashcards = flashcards.Count(f => f.ReviewCount > 0 && (double)f.SuccessCount / f.ReviewCount < 0.4)
        };

        // Calculate streak
        var reviewDates = flashcards
            .SelectMany(f => Enumerable.Range(0, f.ReviewCount)
                .Select(i => f.LastReviewDate.AddDays(-i).Date))
            .Distinct()
            .OrderByDescending(d => d)
            .ToList();

        stats.StreakDays = CalculateStreak(reviewDates);
        stats.ReviewsByDay = reviewDates
            .GroupBy(d => d.ToString("yyyy-MM-dd"))
            .ToDictionary(g => g.Key, g => g.Count());

        return stats;
    }

    private int CalculateStreak(List<DateTime> reviewDates)
    {
        if (!reviewDates.Any()) return 0;

        var streak = 0;
        var currentDate = DateTime.UtcNow.Date;

        foreach (var date in reviewDates)
        {
            if (date.Date == currentDate)
            {
                streak++;
                currentDate = currentDate.AddDays(-1);
            }
            else break;
        }

        return streak;
    }

    private DateTime CalculateNextReviewDate(Flashcard flashcard, bool isCorrect)
    {
        var now = DateTime.UtcNow;
        
        // If incorrect, review again soon
        if (!isCorrect)
        {
            return now.AddHours(1);
        }

        // Exponential backoff for correct answers
        var interval = flashcard.ReviewCount switch
        {
            0 => TimeSpan.FromHours(24),
            1 => TimeSpan.FromDays(3),
            2 => TimeSpan.FromDays(7),
            3 => TimeSpan.FromDays(14),
            4 => TimeSpan.FromDays(30),
            _ => TimeSpan.FromDays(60)
        };

        return now.Add(interval);
    }
} 