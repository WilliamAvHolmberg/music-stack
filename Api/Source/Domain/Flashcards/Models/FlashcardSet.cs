using System.Text.Json.Serialization;
using Api.Study;

namespace Api.Flashcards;

public class FlashcardSet
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }
    
    public int? StudyStructureId { get; set; }
    
    public ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
    
    [JsonIgnore]
    public StudyStructure? StudyStructure { get; set; }
} 