using System.Text.Json;
using Api.Study;
using Api.Study.Dtos;
using Api.Exceptions;

namespace Api.AI;

public interface ITextAnalysisService
{
    Task<StudyStructure> AnalyzeTextAsync(string title, string content, string model);
}

public class TextAnalysisService : ITextAnalysisService
{
    private readonly IAIService _aiService;
    private readonly IPromptService _promptService;
    private readonly ILogger<TextAnalysisService> _logger;

    public TextAnalysisService(
        IAIService aiService,
        IPromptService promptService,
        ILogger<TextAnalysisService> logger)
    {
        _aiService = aiService;
        _promptService = promptService;
        _logger = logger;
    }

    public async Task<StudyStructure> AnalyzeTextAsync(string title, string content, string model)
    {
        try
        {
            var request = _promptService.CreateConceptAnalysisRequest(content, model);
            var response = await _aiService.SendMessageAsync(request);
            
            _logger.LogInformation("Received AI response for model {Model}", model);

            var analysisResult = JsonSerializer.Deserialize<AnalysisResponse>(
                response.Content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (analysisResult == null)
            {
                throw new Exception($"Failed to parse AI response for model {model}");
            }

            _logger.LogInformation("Parsed {ConceptCount} concepts and {RelationshipCount} relationships", 
                analysisResult.Concepts.Length, analysisResult.Relationships.Length);

            // Validate relationships before creating them
            foreach (var rel in analysisResult.Relationships)
            {
                if (rel.SourceConcept < 0 || rel.SourceConcept >= analysisResult.Concepts.Length)
                {
                    _logger.LogError("Invalid source concept index {Index} for model {Model}. Total concepts: {Total}", 
                        rel.SourceConcept, model, analysisResult.Concepts.Length);
                    throw new Exception($"Invalid relationship source index: {rel.SourceConcept}");
                }
                if (rel.TargetConcept < 0 || rel.TargetConcept >= analysisResult.Concepts.Length)
                {
                    _logger.LogError("Invalid target concept index {Index} for model {Model}. Total concepts: {Total}", 
                        rel.TargetConcept, model, analysisResult.Concepts.Length);
                    throw new Exception($"Invalid relationship target index: {rel.TargetConcept}");
                }
            }

            var concepts = analysisResult.Concepts.Select(c => new Concept
            {
                Content = c.Content,
                Level = c.Level,
                Order = c.Order
            }).ToList();

            var structure = new StudyStructure
            {
                Title = title,
                OriginalContent = content,
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                Concepts = concepts
            };

            foreach (var rel in analysisResult.Relationships)
            {
                structure.Relationships.Add(new Relationship
                {
                    SourceConcept = concepts[rel.SourceConcept],
                    TargetConcept = concepts[rel.TargetConcept],
                    Type = rel.Type
                });
            }

            return structure;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing text content with model {Model}", model);
            throw;
        }
    }
} 