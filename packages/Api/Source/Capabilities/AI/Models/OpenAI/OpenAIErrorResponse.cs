using System.Text.Json.Serialization;

namespace Api.AI.OpenAI;

public record OpenAIErrorResponse
{
    [JsonPropertyName("error")]
    public OpenAIError? Error { get; init; }
}

public record OpenAIError
{
    [JsonPropertyName("message")]
    public string Message { get; init; } = "";

    [JsonPropertyName("type")]
    public string Type { get; init; } = "";

    [JsonPropertyName("code")]
    public string? Code { get; init; }

    [JsonPropertyName("param")]
    public string? Param { get; init; }
} 