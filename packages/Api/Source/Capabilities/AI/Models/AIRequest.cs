namespace Api.AI;

public record AIRequest
{
    public required string Model { get; init; }
    public required IEnumerable<AIMessage> Messages { get; init; }
    public int? MaxTokens { get; init; }
    // Add other common parameters as needed
} 