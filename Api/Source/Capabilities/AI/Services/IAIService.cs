using Api.AI;

namespace Api.AI;

public interface IAIService
{
    Task<AIResponse> SendMessageAsync(AIRequest request);
} 