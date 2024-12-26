namespace Api.Study.Dtos;

public class FlashcardGenerationResponse
{
    public FlashcardDto[] Flashcards { get; set; } = Array.Empty<FlashcardDto>();
}

public class FlashcardDto
{
    public string Question { get; set; } = "";
    public string Answer { get; set; } = "";
    public int Importance { get; set; }
} 