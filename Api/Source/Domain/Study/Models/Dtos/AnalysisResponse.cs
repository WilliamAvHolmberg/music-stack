using System.Text.Json.Serialization;

namespace Api.Study.Dtos;

public class AnalysisResponse
{
    [JsonPropertyName("concepts")]
    public ConceptDto[] Concepts { get; set; } = Array.Empty<ConceptDto>();

    [JsonPropertyName("relationships")]
    public RelationshipDto[] Relationships { get; set; } = Array.Empty<RelationshipDto>();
}

public class ConceptDto
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = "";

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }
}

public class RelationshipDto
{
    [JsonPropertyName("sourceConcept")]
    public int SourceConcept { get; set; }

    [JsonPropertyName("targetConcept")]
    public int TargetConcept { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "";
} 