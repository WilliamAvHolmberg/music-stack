using System.ComponentModel.DataAnnotations;

namespace Api.Study;

public class Concept
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public int Level { get; set; }
    public int Order { get; set; }
    public int StudyStructureId { get; set; }
    public StudyStructure StudyStructure { get; set; } = null!;
} 