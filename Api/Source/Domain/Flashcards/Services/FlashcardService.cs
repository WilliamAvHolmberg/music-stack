using Api.Infrastructure;
using Api.Flashcards;
using Microsoft.EntityFrameworkCore;
using Api.Study;
using Api.Study.Dtos;
using System.Text.Json;
using System.Text.Encodings.Web;
using Api.AI;
using Api.Reviews;

namespace Api.Flashcards;

public interface IFlashcardService
{
    Task<FlashcardSet> GenerateFromStudyStructureAsync(int studyStructureId, string title, string model, string provider);
    Task<FlashcardSet> GenerateFromContentAsync(string content, string title, string model, string provider);
    Task<FlashcardSet> GetFlashcardSetAsync(int id);
    Task<IEnumerable<FlashcardSet>> GetAllFlashcardSetsAsync();
    Task<Flashcard> UpdateFlashcardAsync(int id, string question, string answer, int importance);
    Task DeleteFlashcardSetAsync(int id);
    Task<AnswerAnalysis> AnalyzeAnswerAsync(int flashcardId, string userAnswer, string model, string provider);
    Task<IEnumerable<Flashcard>> GetDueFlashcardsAsync(int flashcardSetId);
    Task<ReviewStatistics> GetReviewStatisticsAsync(int flashcardSetId);
} 

public class FlashcardService : IFlashcardService
{
    private readonly AppDbContext _context;
    private readonly IAIService _aiService;
    private readonly IPromptService _promptService;
    private readonly IReviewService _reviewService;
    private readonly ILogger<FlashcardService> _logger;

    public FlashcardService(
        AppDbContext context,
        IAIService aiService,
        IPromptService promptService,
        IReviewService reviewService,
        ILogger<FlashcardService> logger)
    {
        _context = context;
        _aiService = aiService;
        _promptService = promptService;
        _reviewService = reviewService;
        _logger = logger;
    }

    public async Task<FlashcardSet> GenerateFromStudyStructureAsync(int studyStructureId, string title, string model, string provider)
    {
        var structure = await _context.StudyStructures
            .Include(s => s.Concepts)
            .FirstOrDefaultAsync(s => s.Id == studyStructureId)
            ?? throw new Exception("Study structure not found");

        var flashcardSet = new FlashcardSet
        {
            Title = title,
            Description = $"Generated from study structure: {structure.Title}",
            CreatedAt = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            StudyStructureId = studyStructureId
        };

        foreach (var concept in structure.Concepts)
        {
            var request = _promptService.CreateFlashcardRequest(concept.Content, model);
            var response = await _aiService.SendMessageAsync(request);

            var flashcardData = JsonSerializer.Deserialize<FlashcardGenerationResponse>(
                response.Content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (flashcardData?.Flashcards.FirstOrDefault() is FlashcardDto cardData)
            {
                flashcardSet.Flashcards.Add(new Flashcard
                {
                    Question = cardData.Question,
                    Answer = cardData.Answer,
                    Importance = cardData.Importance,
                    CreatedAt = DateTime.UtcNow,
                    ConceptId = concept.Id
                });
            }
        }

        _context.FlashcardSets.Add(flashcardSet);
        await _context.SaveChangesAsync();

        return flashcardSet;
    }

    public async Task<FlashcardSet> GenerateFromContentAsync(string content, string title, string model, string provider)
    {
        var request = _promptService.CreateFlashcardRequest(content, model);
        var response = await _aiService.SendMessageAsync(request);

        try
        {
            _logger.LogInformation("Raw AI response: {Response}", response.Content);

            var cleanedResponse = CleanJsonResponse(response.Content);
            _logger.LogInformation("Cleaned response: {Response}", cleanedResponse);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var flashcardData = JsonSerializer.Deserialize<FlashcardGenerationResponse>(
                cleanedResponse,
                options
            );

            if (flashcardData == null || flashcardData.Flashcards == null || !flashcardData.Flashcards.Any())
            {
                throw new Exception("No flashcards were generated from the content");
            }

            var flashcardSet = new FlashcardSet
            {
                Title = title,
                Description = "Generated directly from content",
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            foreach (var cardData in flashcardData.Flashcards)
            {
                flashcardSet.Flashcards.Add(new Flashcard
                {
                    Question = cardData.Question,
                    Answer = cardData.Answer,
                    Importance = cardData.Importance,
                    CreatedAt = DateTime.UtcNow
                });
            }

            _context.FlashcardSets.Add(flashcardSet);
            await _context.SaveChangesAsync();

            return flashcardSet;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse AI response: {Response}", response.Content);
            throw new Exception("Failed to parse AI response", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating flashcards: {Response}", response.Content);
            throw;
        }
    }

    private string CleanJsonResponse(string response)
    {
        try
        {
            // Remove any markdown code block markers
            response = response.Replace("```json", "").Replace("```", "").Trim();

            // Find the first { and last }
            var start = response.IndexOf('{');
            var end = response.LastIndexOf('}');

            if (start >= 0 && end > start)
            {
                return response.Substring(start, end - start + 1);
            }

            throw new Exception("No valid JSON object found in response");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning JSON response: {Response}", response);
            throw;
        }
    }

    public async Task<FlashcardSet> GetFlashcardSetAsync(int id)
    {
        return await _context.FlashcardSets
            .Include(f => f.Flashcards)
            .Include(f => f.StudyStructure)
            .FirstOrDefaultAsync(f => f.Id == id)
            ?? throw new Exception("Flashcard set not found");
    }

    public async Task<IEnumerable<FlashcardSet>> GetAllFlashcardSetsAsync()
    {
        return await _context.FlashcardSets
            .Include(f => f.Flashcards)
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync();
    }

    public async Task<Flashcard> UpdateFlashcardAsync(int id, string question, string answer, int importance)
    {
        var flashcard = await _context.Flashcards.FindAsync(id)
            ?? throw new Exception("Flashcard not found");

        flashcard.Question = question;
        flashcard.Answer = answer;
        flashcard.Importance = importance;

        await _context.SaveChangesAsync();
        return flashcard;
    }

    public async Task DeleteFlashcardSetAsync(int id)
    {
        var flashcardSet = await _context.FlashcardSets.FindAsync(id)
            ?? throw new Exception("Flashcard set not found");

        _context.FlashcardSets.Remove(flashcardSet);
        await _context.SaveChangesAsync();
    }

    public async Task<AnswerAnalysis> AnalyzeAnswerAsync(int flashcardId, string userAnswer, string model, string provider)
    {
        var flashcard = await _context.Flashcards.FindAsync(flashcardId)
            ?? throw new Exception("Flashcard not found");

        var request = _promptService.CreateAnswerAnalysisRequest(flashcard.Question, flashcard.Answer, userAnswer, model);
        var response = await _aiService.SendMessageAsync(request);

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var cleanedResponse = CleanJsonResponse(response.Content);
            var analysis = JsonSerializer.Deserialize<AnswerAnalysis>(cleanedResponse, options);
            
            if (analysis == null)
            {
                throw new Exception("Failed to parse AI response");
            }

            return analysis;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error parsing AI response");
            throw new Exception("Failed to analyze answer", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing answer for flashcard {Id}", flashcardId);
            throw new Exception("Failed to analyze answer", ex);
        }
    }

    public async Task<IEnumerable<Flashcard>> GetDueFlashcardsAsync(int flashcardSetId)
    {
        return await _reviewService.GetFlashcardsForStudyAsync(flashcardSetId);
    }

    public async Task<ReviewStatistics> GetReviewStatisticsAsync(int flashcardSetId)
    {
        return await _reviewService.GetStudyStatisticsAsync(flashcardSetId);
    }
} 