using Api.Domain.Games.Models;

namespace Api.Domain.Games.Repositories;

public interface IGameRepository
{
    Task<GameSession?> GetByIdAsync(int id);
    Task<IEnumerable<GameSession>> GetActiveGamesAsync();
    Task<GameSession> CreateGameAsync(GameSession game);
    Task UpdateGameAsync(GameSession game);
    Task DeleteGameAsync(int id);
    
    // Team operations
    Task<Team> AddTeamAsync(Team team);
    Task UpdateTeamAsync(Team team);
    Task<IEnumerable<Team>> GetTeamsByGameIdAsync(int gameId);
    
    // Round operations
    Task<Round> AddRoundAsync(Round round);
    Task UpdateRoundAsync(Round round);
    Task<IEnumerable<Round>> GetRoundsByGameIdAsync(int gameId);
    Task<Round?> GetCurrentRoundAsync(int gameId);
    Task<RoundItem> AddRoundItemAsync(RoundItem item);
    Task<GameTemplate?> GetTemplateAsync(int id);
    Task RevealAnswerAsync(int gameId, int itemId);
} 