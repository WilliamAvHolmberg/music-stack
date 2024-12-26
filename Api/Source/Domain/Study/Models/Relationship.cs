using System.ComponentModel.DataAnnotations;

namespace Api.Study;

public class Relationship
{
    public int Id { get; set; }
    public int StudyStructureId { get; set; }
    public StudyStructure StudyStructure { get; set; } = null!;
    public int SourceConceptId { get; set; }
    public Concept SourceConcept { get; set; } = null!;
    public int TargetConceptId { get; set; }
    public Concept TargetConcept { get; set; } = null!;
    public string Type { get; set; } = "relates_to";
} 