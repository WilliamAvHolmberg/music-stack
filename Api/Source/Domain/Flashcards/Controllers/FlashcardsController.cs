using Microsoft.AspNetCore.Mvc;
using Api.Flashcards;
using Api.Study;
using Api.Reviews;

namespace Api.Flashcards;

[ApiController]
[Route("api/[controller]")]
public class FlashcardsController : ControllerBase
{
    private readonly IFlashcardService _flashcardService;
    private readonly ILogger<FlashcardsController> _logger;

    public FlashcardsController(
        IFlashcardService flashcardService,
        ILogger<FlashcardsController> logger)
    {
        _flashcardService = flashcardService;
        _logger = logger;
    }

    [HttpPost("generate/structure/{studyStructureId}")]
    public async Task<ActionResult<FlashcardSet>> GenerateFromStudyStructure(
        int studyStructureId,
        [FromQuery] string title,
        [FromQuery] string model = "gpt-4",
        [FromQuery] string provider = "openai")
    {
        try
        {
            var flashcardSet = await _flashcardService.GenerateFromStudyStructureAsync(studyStructureId, title, model, provider);
            return Ok(flashcardSet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate flashcards from study structure");
            return StatusCode(500, "Failed to generate flashcards");
        }
    }

    [HttpPost("generate/content")]
    public async Task<ActionResult<FlashcardSet>> GenerateFromContent(
        [FromBody] GenerateFlashcardsRequest request)
    {
        try
        {
            var flashcardSet = await _flashcardService.GenerateFromContentAsync(
                request.Content, 
                request.Title, 
                request.Model, 
                request.Provider);
            return Ok(flashcardSet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate flashcards from content");
            return StatusCode(500, "Failed to generate flashcards");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FlashcardSet>> GetFlashcardSet(int id)
    {
        try
        {
            var flashcardSet = await _flashcardService.GetFlashcardSetAsync(id);
            return Ok(flashcardSet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get flashcard set");
            return StatusCode(500, "Failed to get flashcard set");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FlashcardSet>>> GetAllFlashcardSets()
    {
        try
        {
            var flashcardSets = await _flashcardService.GetAllFlashcardSetsAsync();
            return Ok(flashcardSets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all flashcard sets");
            return StatusCode(500, "Failed to get flashcard sets");
        }
    }

    [HttpPut("flashcard/{id}")]
    public async Task<ActionResult<Flashcard>> UpdateFlashcard(
        int id,
        [FromQuery] string question,
        [FromQuery] string answer,
        [FromQuery] int importance)
    {
        try
        {
            var flashcard = await _flashcardService.UpdateFlashcardAsync(id, question, answer, importance);
            return Ok(flashcard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update flashcard");
            return StatusCode(500, "Failed to update flashcard");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFlashcardSet(int id)
    {
        try
        {
            await _flashcardService.DeleteFlashcardSetAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete flashcard set");
            return StatusCode(500, "Failed to delete flashcard set");
        }
    }

    [HttpPost("flashcard/{id}/analyze")]
    public async Task<ActionResult<AnswerAnalysis>> AnalyzeAnswer(
        int id,
        [FromBody] string userAnswer,
        [FromQuery] string model = "gpt-4",
        [FromQuery] string provider = "openai")
    {
        try
        {
            var analysis = await _flashcardService.AnalyzeAnswerAsync(id, userAnswer, model, provider);
            return Ok(analysis);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to analyze answer");
            return StatusCode(500, "Failed to analyze answer");
        }
    }

    [HttpGet("{id}/due")]
    public async Task<ActionResult<IEnumerable<Flashcard>>> GetDueFlashcards(int id)
    {
        try
        {
            var flashcards = await _flashcardService.GetDueFlashcardsAsync(id);
            return Ok(flashcards);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get due flashcards");
            return StatusCode(500, "Failed to get due flashcards");
        }
    }

    [HttpGet("{id}/statistics")]
    public async Task<ActionResult<ReviewStatistics>> GetReviewStatistics(int id)
    {
        try
        {
            var statistics = await _flashcardService.GetReviewStatisticsAsync(id);
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get review statistics");
            return StatusCode(500, "Failed to get review statistics");
        }
    }
} 