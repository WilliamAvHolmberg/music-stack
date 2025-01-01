using Api.Domain.Games.Models;

namespace Api.Domain.Games.Repositories;

public interface IGameTemplateRepository
{
    Task<IEnumerable<GameTemplate>> GetAllAsync();
    Task<GameTemplate?> GetByIdAsync(int id);
    Task<GameTemplate> UpsertAsync(GameTemplate template);
    Task DeleteAsync(int id);
} 