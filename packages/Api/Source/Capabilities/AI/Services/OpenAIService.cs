using System.Text.Json;
using System.Text;
using Api.AI;
using Api.AI.OpenAI;
using Api.Exceptions;
using Api.Configuration;

namespace Api.AI;

public class OpenAIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly AppSettings _settings;
    private readonly ILogger<OpenAIService> _logger;
    private const string OPENAI_API_URL = "https://api.openai.com/v1/chat/completions";

    public OpenAIService(
        HttpClient httpClient,
        AppSettings settings,
        ILogger<OpenAIService> logger)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.ApiKeys.OpenAiKey}");
    }

    public async Task<AIResponse> SendMessageAsync(AIRequest request)
    {
        try
        {
            // Handle system messages by prepending to first user message
            var messages = new List<OpenAIMessage>();
            var systemMessage = request.Messages.FirstOrDefault(m => m.Role == AIMessageRole.System);
            
            foreach (var message in request.Messages.Where(m => m.Role != AIMessageRole.System))
            {
                var content = message.Content;
                if (systemMessage != null && message == request.Messages.First(m => m.Role == AIMessageRole.User))
                {
                    content = $"{systemMessage.Content}\n\n{content}";
                }
                
                messages.Add(new OpenAIMessage(MapRole(message.Role), content));
            }

            var openAIRequest = new OpenAIRequest
            {
                Model = request.Model,
                MaxTokens = request.MaxTokens,
                Messages = messages
            };

            var response = await _httpClient.PostAsJsonAsync(OPENAI_API_URL, openAIRequest);
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("OpenAI error: {Error}", responseBody);
                throw new Exception($"OpenAI API error: {responseBody}");
            }

            var openAIResponse = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
            
            return new AIResponse
            {
                Content = openAIResponse?.Choices.FirstOrDefault()?.Message.Content 
                    ?? throw new Exception("Empty response from OpenAI"),
                Model = request.Model
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in OpenAI service");
            throw;
        }
    }

    private string MapRole(AIMessageRole role) => role switch
    {
        AIMessageRole.System => throw new ArgumentException("OpenAI doesn't support system messages directly"),
        AIMessageRole.User => "user",
        AIMessageRole.Assistant => "assistant",
        _ => throw new ArgumentException($"Unknown role: {role}")
    };
} 