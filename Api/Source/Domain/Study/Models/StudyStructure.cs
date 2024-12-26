using System.ComponentModel.DataAnnotations;

namespace Api.Study;

public class StudyStructure
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string OriginalContent { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }
    public List<Concept> Concepts { get; set; } = new();
    public List<Relationship> Relationships { get; set; } = new();
} 