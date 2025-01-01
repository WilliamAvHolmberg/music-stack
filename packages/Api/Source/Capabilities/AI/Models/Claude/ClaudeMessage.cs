namespace Api.AI.Claude;
using System.Text.Json.Serialization;

public record ClaudeMessage(string Role, string Content);

public record ClaudeRequest
{
    [JsonPropertyName("model")]
    public required string Model { get; init; }

    [JsonPropertyName("messages")]
    public required IEnumerable<ClaudeMessage> Messages { get; init; }

    [JsonPropertyName("max_tokens")]
    public required int MaxTokens { get; init; }
}

public record ClaudeResponse
{
    public required ClaudeResponseContent[] Content { get; init; }
    
    public record ClaudeResponseContent
    {
        public required string Text { get; init; }
    }
} 