using Microsoft.AspNetCore.Mvc;
using Api.Study;
using Api.AI;
using Api.Flashcards;

namespace Api.Reviews;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly ILogger<ReviewController> _logger;

    public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger)
    {
        _reviewService = reviewService;
        _logger = logger;
    }

    [HttpGet("flashcard-set/{id}/due")]
    public async Task<ActionResult<IEnumerable<Flashcard>>> GetDueFlashcards(int id)
    {
        try
        {
            return Ok(await _reviewService.GetFlashcardsForStudyAsync(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting due flashcards for set {Id}", id);
            return StatusCode(500, "Error getting due flashcards");
        }
    }

    [HttpPost("flashcard/{id}/review")]
    public async Task<ActionResult<ReviewStatistics>> ProcessReview(
        int id, 
        [FromBody] ReviewRequest request)
    {
        try
        {
            var statistics = await _reviewService.ProcessAnswerAsync(
                id, 
                request.IsCorrect);
                
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing review for flashcard {Id}", id);
            return StatusCode(500, "Error processing review");
        }
    }

    [HttpGet("flashcard-set/{id}/statistics")]
    public async Task<ActionResult<ReviewStatistics>> GetStatistics(int id)
    {
        try
        {
            return Ok(await _reviewService.GetStudyStatisticsAsync(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting statistics for set {Id}", id);
            return StatusCode(500, "Error getting statistics");
        }
    }
}

public record ReviewRequest(bool IsCorrect, float Confidence); 