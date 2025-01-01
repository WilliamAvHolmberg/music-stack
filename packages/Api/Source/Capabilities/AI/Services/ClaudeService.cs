using Api.AI;
using Api.AI.Claude;
using Api.Configuration;

namespace Api.AI;

public class ClaudeService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly AppSettings _settings;
    private readonly ILogger<ClaudeService> _logger;

    public ClaudeService(
        HttpClient httpClient,
        AppSettings settings,
        ILogger<ClaudeService> logger)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;

        _httpClient.BaseAddress = new Uri("https://api.anthropic.com/v1/");
        _httpClient.DefaultRequestHeaders.Add("x-api-key", settings.ApiKeys.ClaudeKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
    }

    public async Task<AIResponse> SendMessageAsync(AIRequest request)
    {
        try
        {
            var messages = new List<ClaudeMessage>();
            var systemMessage = request.Messages.FirstOrDefault(m => m.Role == AIMessageRole.System);
            
            foreach (var message in request.Messages.Where(m => m.Role != AIMessageRole.System))
            {
                var content = message.Content;
                if (systemMessage != null && message == request.Messages.First(m => m.Role == AIMessageRole.User))
                {
                    content = $"{systemMessage.Content}\n\n{content}";
                }
                
                messages.Add(new ClaudeMessage(MapRole(message.Role), content));
            }

            var claudeRequest = new ClaudeRequest
            {
                Model = request.Model,
                MaxTokens = 4096,
                Messages = messages
            };

            var response = await _httpClient.PostAsJsonAsync("messages", claudeRequest);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Claude error: {Error}", responseBody);
                throw new Exception($"Claude API error: {responseBody}");
            }

            var claudeResponse = await response.Content.ReadFromJsonAsync<ClaudeResponse>();
            
            return new AIResponse
            {
                Content = claudeResponse?.Content.FirstOrDefault()?.Text 
                    ?? throw new Exception("Empty response from Claude"),
                Model = request.Model
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Claude service");
            throw;
        }
    }

    private string MapRole(AIMessageRole role) => role switch
    {
        AIMessageRole.System => throw new ArgumentException("Claude doesn't support system messages directly"),
        AIMessageRole.User => "user",
        AIMessageRole.Assistant => "assistant",
        _ => throw new ArgumentException($"Unknown role: {role}")
    };
} 