using Api.Data;
using Api.Models.Study;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Tests.Services;

public class ReviewServiceTests : TestBase
{
    private readonly IReviewService _reviewService;
    private readonly AppDbContext _context;
    
    public ReviewServiceTests()
    {
        var scope = _factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ReviewService>>();
        _reviewService = new ReviewService(_context, logger);
        
        // Seed test data
        SeedTestData().Wait();
    }
    
    private async Task SeedTestData()
    {
        var flashcardSet = new FlashcardSet
        {
            Title = "Test Set",
            Description = "Test Description"
        };
        _context.FlashcardSets.Add(flashcardSet);
        await _context.SaveChangesAsync();
        
        var flashcards = new[]
        {
            new Flashcard
            {
                FlashcardSetId = flashcardSet.Id,
                Question = "Test Question 1",
                Answer = "Test Answer 1",
                CreatedAt = DateTime.UtcNow
            },
            new Flashcard
            {
                FlashcardSetId = flashcardSet.Id,
                Question = "Test Question 2",
                Answer = "Test Answer 2",
                CreatedAt = DateTime.UtcNow
            }
        };
        
        _context.Flashcards.AddRange(flashcards);
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task GetFlashcardsForStudy_ReturnsAllFlashcards()
    {
        // Arrange
        var flashcardSet = await _context.FlashcardSets.FirstOrDefaultAsync() 
            ?? throw new Exception("Test data not seeded properly");
        
        // Act
        var flashcards = await _reviewService.GetFlashcardsForStudyAsync(flashcardSet.Id);
        
        // Assert
        Assert.Equal(2, flashcards.Count());
    }
    
    [Fact]
    public async Task ProcessAnswer_CreatesReviewAndUpdatesStatistics()
    {
        // Arrange
        var flashcard = await _context.Flashcards.FirstOrDefaultAsync()
            ?? throw new Exception("Test data not seeded properly");
        
        // Act
        var stats = await _reviewService.ProcessAnswerAsync(flashcard.Id, true);
        
        // Assert
        Assert.Equal(1, stats.TotalAttempts);
        Assert.Equal(1, stats.CorrectAnswers);
        Assert.Equal(0, stats.IncorrectAnswers);
        
        // Verify review was created
        var review = await _context.FlashcardReviews.FirstOrDefaultAsync()
            ?? throw new Exception("Review was not created");
        Assert.Equal(flashcard.Id, review.FlashcardId);
        Assert.True(review.IsCorrect);
    }
    
    [Fact]
    public async Task GetStudyStatistics_CalculatesCorrectly()
    {
        // Arrange
        var flashcard = await _context.Flashcards.FirstOrDefaultAsync()
            ?? throw new Exception("Test data not seeded properly");
        var flashcardSet = await _context.FlashcardSets.FirstOrDefaultAsync()
            ?? throw new Exception("Test data not seeded properly");
        
        // Create some reviews
        await _reviewService.ProcessAnswerAsync(flashcard.Id, true);
        await _reviewService.ProcessAnswerAsync(flashcard.Id, false);
        
        // Act
        var stats = await _reviewService.GetStudyStatisticsAsync(flashcardSet.Id);
        
        // Assert
        Assert.Equal(2, stats.TotalCards);
        Assert.Equal(2, stats.TotalAttempts);
        Assert.Equal(1, stats.CorrectAnswers);
        Assert.Equal(1, stats.IncorrectAnswers);
        Assert.Equal(2, stats.MostDifficultCards.Count); // Both cards are returned since they have no incorrect answers
        Assert.Contains(flashcard.Id, stats.MostDifficultCards.Select(c => c.Id));
    }
} 