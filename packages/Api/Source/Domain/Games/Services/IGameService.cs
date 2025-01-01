using Api.Domain.Games.Models;
using Api.Domain.Games.Models.Requests;
using Api.Domain.Games.Models.Responses;

namespace Api.Domain.Games.Services;

public interface IGameService
{
    Task<GameSessionResponse> CreateGameAsync(int templateId);
    Task<GameSessionResponse?> GetGameAsync(int id);
    Task<IEnumerable<GameSessionResponse>> GetActiveGamesAsync();
    Task<GameSessionResponse> StartGameAsync(int id);
    Task<TeamResponse> AddTeamAsync(int gameId, CreateTeamRequest request);
    Task<RoundResponse> StartRoundAsync(int gameId, CreateRoundRequest request);
    Task<RoundResponse> EndRoundAsync(int gameId);
    Task<GameSessionResponse> EndGameAsync(int id);
    Task<TeamResponse> UpdateTeamScoreAsync(int gameId, int teamId, int score);
    Task DeleteGameAsync(int id);

    Task<GameSessionResponse> NextItemAsync(int gameId);
    Task<GameSessionResponse> PreviousItemAsync(int gameId);
    Task<GameSessionResponse> PauseTimerAsync(int gameId);
    Task<GameSessionResponse> ResumeTimerAsync(int gameId);
    Task<GameSessionResponse> ResetTimerAsync(int gameId);
    Task<GameSessionResponse> UpdateGameStateAsync(int gameId, UpdateGameStateRequest request);
    Task RevealAnswerAsync(int gameId, int itemId);
} 