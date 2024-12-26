using Microsoft.AspNetCore.Mvc;
using Api.Study;
using Api.Study.Dtos;
using Api.AI;
using Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Api.Exceptions;
using System.Text.Json;

namespace Api.Study;

[ApiController]
[Route("api/[controller]")]
public class StudyController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<StudyController> _logger;
    private readonly IAIService _aiService;
    private readonly IPromptService _promptService;

    public StudyController(
        AppDbContext context,
        ILogger<StudyController> logger,
        IAIService aiService,
        IPromptService promptService)
    {
        _context = context;
        _logger = logger;
        _aiService = aiService;
        _promptService = promptService;
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<StudyStructure>> AnalyzeContent([FromBody] AnalyzeRequest request)
    {
        try
        {
            _logger.LogInformation("Analyzing content with model {Model}", request.Model);

            var structure = new StudyStructure
            {
                Title = request.Title,
                OriginalContent = request.Content,
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };

            // Create AI request using prompt service
            var aiRequest = _promptService.CreateConceptAnalysisRequest(request.Content, request.Model);
            var aiResponse = await _aiService.SendMessageAsync(aiRequest);

            var analysisResult = JsonSerializer.Deserialize<AnalysisResponse>(
                aiResponse.Content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (analysisResult == null)
            {
                throw new Exception($"Failed to parse AI response for model {request.Model}");
            }

            // Create concepts and relationships
            structure.Concepts = analysisResult.Concepts.Select(c => new Concept
            {
                Content = c.Content,
                Level = c.Level,
                Order = c.Order
            }).ToList();

            foreach (var rel in analysisResult.Relationships)
            {
                var sourceConcept = structure.Concepts.ElementAtOrDefault(rel.SourceConcept);
                var targetConcept = structure.Concepts.ElementAtOrDefault(rel.TargetConcept);
                
                if (sourceConcept == null || targetConcept == null)
                {
                    throw new Exception($"Invalid relationship indices: {rel.SourceConcept} -> {rel.TargetConcept}");
                }

                structure.Relationships.Add(new Relationship
                {
                    SourceConcept = sourceConcept,
                    TargetConcept = targetConcept,
                    Type = rel.Type
                });
            }

            _context.StudyStructures.Add(structure);
            await _context.SaveChangesAsync();
            
            return Ok(structure);
        }
        catch (OpenAIException ex)
        {
            _logger.LogError(ex, "AI API error: Type={ErrorType}, Code={ErrorCode}, Message={Message}", 
                ex.ErrorType, ex.ErrorCode, ex.Message);
            
            return StatusCode(503, new 
            { 
                message = ex.Message,
                details = new { type = ex.ErrorType, code = ex.ErrorCode }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing content with model {Model}: {Message}", 
                request.Model, ex.Message);
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudyStructure>>> GetStructures()
    {
        return await _context.StudyStructures
            .Include(s => s.Concepts)
            .Include(s => s.Relationships)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudyStructure>> GetStructure(int id)
    {
        var structure = await _context.StudyStructures
            .Include(s => s.Concepts)
            .Include(s => s.Relationships)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (structure == null) return NotFound();
        return structure;
    }
}

public class AnalyzeRequest
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Model { get; set; }
    public required string Provider { get; set; }
} 