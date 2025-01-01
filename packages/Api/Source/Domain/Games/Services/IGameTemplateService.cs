using Api.Domain.Games.Models.Requests;
using Api.Domain.Games.Models.Responses;

namespace Api.Domain.Games.Services;

public interface IGameTemplateService
{
    Task<IEnumerable<GameTemplateResponse>> GetTemplatesAsync();
    Task<GameTemplateResponse?> GetTemplateAsync(int id);
    Task<GameTemplateResponse> UpsertTemplateAsync(int? id, CreateGameTemplateRequest request);
    Task DeleteTemplateAsync(int id);
} 