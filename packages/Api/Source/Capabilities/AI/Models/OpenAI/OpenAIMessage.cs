using System.Text.Json.Serialization;

namespace Api.AI.OpenAI;

public record OpenAIMessage(string Role, string Content); 