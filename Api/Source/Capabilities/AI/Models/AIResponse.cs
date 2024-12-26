namespace Api.AI;

public record AIResponse
{
    public required string Content { get; init; }
    public required string Model { get; init; }
    // Add other common response fields as needed
} 