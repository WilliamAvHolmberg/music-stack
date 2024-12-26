namespace Api.Study.Dtos;

public class GenerateFromContentRequest
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Model { get; set; }
    public required string Provider { get; set; }
}

public class GenerateFromStructureRequest
{
    public required string Title { get; set; }
    public required int StudyStructureId { get; set; }
    public required string Model { get; set; }
    public required string Provider { get; set; }
}

public class UpdateFlashcardRequest
{
    public required string Question { get; set; }
    public required string Answer { get; set; }
    public required int Importance { get; set; }
} 