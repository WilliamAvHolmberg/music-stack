using Api.Domain.Games.Models;
using Api.Domain.Games.Models.Requests;
using Api.Domain.Games.Models.Responses;
using Api.Domain.Games.Repositories;
using Api.Domain.Songs.Services;
using Api.Shared.Infrastructure.Exceptions;

namespace Api.Domain.Games.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _repository;
    private readonly ISongService _songService;
    private readonly ILogger<GameService> _logger;

    public GameService(IGameRepository repository, ISongService songService, ILogger<GameService> logger)
    {
        _repository = repository;
        _songService = songService;
        _logger = logger;
    }

    public async Task<GameSessionResponse> CreateGameAsync(int templateId)
    {
        var template = await _repository.GetTemplateAsync(templateId);
        if (template == null)
        {
            throw new KeyNotFoundException($"Template {templateId} not found");
        }

        var game = new GameSession
        {
            GameTemplateId = templateId,
            Status = GameStatus.NotStarted,
            CreatedAt = DateTime.UtcNow,
            CurrentRoundIndex = 0,
            CurrentItemIndex = 0
        };

        await _repository.CreateGameAsync(game);

        // Copy rounds from template
        foreach (var templateRound in template.Rounds.OrderBy(r => r.OrderIndex))
        {
            var round = new Round
            {
                GameSessionId = game.Id,
                Type = templateRound.Type,
                Status = RoundStatus.NotStarted,
                Title = templateRound.Title,
                TimeInMinutes = templateRound.TimeInMinutes,
                Instructions = templateRound.Instructions,
                TimeLeft = templateRound.TimeInMinutes * 60,
                OrderIndex = templateRound.OrderIndex
            };

            await _repository.AddRoundAsync(round);

            // Copy items from template round
            foreach (var templateItem in templateRound.Items.OrderBy(i => i.OrderIndex))
            {
                var item = new RoundItem
                {
                    RoundId = round.Id,
                    Title = templateItem.Title,
                    Artist = templateItem.Artist,
                    Points = templateItem.Points,
                    ExtraInfo = templateItem.ExtraInfo,
                    OrderIndex = templateItem.OrderIndex,
                    SpotifyId = templateItem.SpotifyId,
                    Year = templateItem.Year
                };

                await _repository.AddRoundItemAsync(item);
            }
        }

        return MapToGameSessionResponse(game);
    }

    public async Task<GameSessionResponse?> GetGameAsync(int id)
    {
        var game = await _repository.GetByIdAsync(id);
        return game != null ? MapToGameSessionResponse(game) : null;
    }

    public async Task<IEnumerable<GameSessionResponse>> GetActiveGamesAsync()
    {
        var games = await _repository.GetActiveGamesAsync();
        return games.Select(MapToGameSessionResponse);
    }

    public async Task<TeamResponse> AddTeamAsync(int gameId, CreateTeamRequest request)
    {
        var team = new Team
        {
            GameSessionId = gameId,
            Name = request.Name,
            Color = request.Color,
            Score = 0
        };

        await _repository.AddTeamAsync(team);
        return MapToTeamResponse(team);
    }

    public async Task<RoundResponse> StartRoundAsync(int gameId, CreateRoundRequest request)
    {
        var game = await _repository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game {gameId} not found");
        }

        var rounds = await _repository.GetRoundsByGameIdAsync(gameId);
        var nextRound = rounds
            .Where(r => r.Status == RoundStatus.NotStarted)
            .OrderBy(r => r.OrderIndex)
            .FirstOrDefault();

        if (nextRound == null)
        {
            throw new InvalidOperationException("No more rounds available");
        }

        nextRound.Status = RoundStatus.InProgress;
        await _repository.UpdateRoundAsync(nextRound);

        game.CurrentRoundIndex = rounds.Count(r => r.Status != RoundStatus.NotStarted) - 1;
        await _repository.UpdateGameAsync(game);

        return MapToRoundResponse(nextRound);
    }

    public async Task<RoundResponse> EndRoundAsync(int gameId)
    {
        var game = await _repository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game {gameId} not found");
        }

        var currentRound = await _repository.GetCurrentRoundAsync(gameId);
        if (currentRound == null)
        {
            throw new InvalidOperationException("No active round found");
        }

        // Mark current round as completed
        currentRound.Status = RoundStatus.Completed;
        await _repository.UpdateRoundAsync(currentRound);

        // Update game status if this was the last round
        var allRounds = await _repository.GetRoundsByGameIdAsync(gameId);
        var hasMoreRounds = allRounds.Any(r => r.Status == RoundStatus.NotStarted);
        
        if (!hasMoreRounds)
        {
            game.Status = GameStatus.Finished;
            await _repository.UpdateGameAsync(game);
        }

        return MapToRoundResponse(currentRound);
    }

    public async Task<GameSessionResponse> EndGameAsync(int id)
    {
        var game = await _repository.GetByIdAsync(id);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game {id} not found");
        }

        game.Status = GameStatus.Finished;
        await _repository.UpdateGameAsync(game);

        return MapToGameSessionResponse(game);
    }

    public async Task<TeamResponse> UpdateTeamScoreAsync(int gameId, int teamId, int score)
    {
        var teams = await _repository.GetTeamsByGameIdAsync(gameId);
        var team = teams.FirstOrDefault(t => t.Id == teamId);
        if (team == null)
        {
            throw new KeyNotFoundException($"Team {teamId} not found");
        }

        team.Score = score;
        await _repository.UpdateTeamAsync(team);

        return MapToTeamResponse(team);
    }

    public async Task DeleteGameAsync(int id)
    {
        var game = await _repository.GetByIdAsync(id);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game {id} not found");
        }

        await _repository.DeleteGameAsync(id);
    }

    public async Task<GameSessionResponse> StartGameAsync(int id)
    {
        var game = await _repository.GetByIdAsync(id);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game {id} not found");
        }

        if (game.Status != GameStatus.NotStarted)
        {
            throw new InvalidOperationException("Game has already started or is finished");
        }

        if (!game.Teams.Any())
        {
            throw new InvalidOperationException("Cannot start game without any teams");
        }

        game.Status = GameStatus.InProgress;
        game.CurrentRoundIndex = 0;
        game.CurrentItemIndex = 0;

        await _repository.UpdateGameAsync(game);
        return MapToGameSessionResponse(game);
    }

    public async Task<GameSessionResponse> NextItemAsync(int gameId)
    {
        var game = await _repository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game {gameId} not found");
        }

        var currentRound = await _repository.GetCurrentRoundAsync(gameId);
        if (currentRound == null)
        {
            throw new InvalidOperationException("No active round found");
        }

        if (game.CurrentItemIndex >= currentRound.Items.Count - 1)
        {
            throw new InvalidOperationException("Already at the last item");
        }

        game.CurrentItemIndex++;
        await _repository.UpdateGameAsync(game);

        return MapToGameSessionResponse(game);
    }

    public async Task<GameSessionResponse> PreviousItemAsync(int gameId)
    {
        var game = await _repository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game {gameId} not found");
        }

        if (game.CurrentItemIndex <= 0)
        {
            throw new InvalidOperationException("Already at the first item");
        }

        game.CurrentItemIndex--;
        await _repository.UpdateGameAsync(game);

        return MapToGameSessionResponse(game);
    }

    public async Task<GameSessionResponse> PauseTimerAsync(int gameId)
    {
        var game = await _repository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game {gameId} not found");
        }

        var currentRound = await _repository.GetCurrentRoundAsync(gameId);
        if (currentRound == null)
        {
            throw new InvalidOperationException("No active round found");
        }

        currentRound.TimeLeft = currentRound.TimeLeft; // Save current time
        currentRound.IsPaused = true;
        await _repository.UpdateRoundAsync(currentRound);

        return MapToGameSessionResponse(game);
    }

    public async Task<GameSessionResponse> ResumeTimerAsync(int gameId)
    {
        var game = await _repository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game {gameId} not found");
        }

        var currentRound = await _repository.GetCurrentRoundAsync(gameId);
        if (currentRound == null)
        {
            throw new InvalidOperationException("No active round found");
        }

        currentRound.IsPaused = false;
        await _repository.UpdateRoundAsync(currentRound);

        return MapToGameSessionResponse(game);
    }

    public async Task<GameSessionResponse> ResetTimerAsync(int gameId)
    {
        var game = await _repository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game {gameId} not found");
        }

        var currentRound = await _repository.GetCurrentRoundAsync(gameId);
        if (currentRound == null)
        {
            throw new InvalidOperationException("No active round found");
        }

        currentRound.TimeLeft = currentRound.TimeInMinutes * 60;
        await _repository.UpdateRoundAsync(currentRound);

        return MapToGameSessionResponse(game);
    }

    public async Task<GameSessionResponse> UpdateGameStateAsync(int gameId, UpdateGameStateRequest request)
    {
        var game = await _repository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new KeyNotFoundException($"Game {gameId} not found");
        }

        var currentRound = await _repository.GetCurrentRoundAsync(gameId);
        if (currentRound == null)
        {
            throw new InvalidOperationException("No active round found");
        }

        // Update game state based on request
        if (request.TimeLeft.HasValue)
        {
            currentRound.TimeLeft = request.TimeLeft.Value;
        }

        if (request.IsPaused.HasValue)
        {
            currentRound.IsPaused = request.IsPaused.Value;
        }

        await _repository.UpdateRoundAsync(currentRound);
        return MapToGameSessionResponse(game);
    }

    public async Task RevealAnswerAsync(int gameId, int itemId)
    {
        var game = await _repository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new NotFoundException($"Game {gameId} not found");
        }

        var currentRound = game.Rounds.FirstOrDefault(r => r.Status == RoundStatus.InProgress);
        if (currentRound == null)
        {
            throw new NotFoundException("No active round found");
        }

        var item = currentRound.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            throw new NotFoundException($"Item {itemId} not found in current round");
        }

        item.IsAnswerRevealed = !item.IsAnswerRevealed;
        await _repository.UpdateGameAsync(game);
    }

    private static GameSessionResponse MapToGameSessionResponse(GameSession game)
    {
        return new GameSessionResponse
        {
            Id = game.Id,
            Status = game.Status,
            CreatedAt = game.CreatedAt,
            CurrentRoundIndex = game.CurrentRoundIndex,
            CurrentItemIndex = game.CurrentItemIndex,
            Teams = game.Teams.Select(MapToTeamResponse).ToList(),
            Rounds = game.Rounds.Select(MapToRoundResponse).ToList()
        };
    }

    private static TeamResponse MapToTeamResponse(Team team)
    {
        return new TeamResponse
        {
            Id = team.Id,
            Name = team.Name,
            Score = team.Score,
            Color = team.Color
        };
    }

    private static RoundResponse MapToRoundResponse(Round round)
    {
        return new RoundResponse
        {
            Id = round.Id,
            Title = round.Title,
            Type = round.Type,
            Status = round.Status,
            TimeInMinutes = round.TimeInMinutes,
            Instructions = round.Instructions,
            OrderIndex = round.OrderIndex,
            IsAnswerRevealed = round.IsAnswerRevealed,
            Items = round.Items
                .OrderBy(i => i.OrderIndex)
                .Select(MapToRoundItemResponse)
                .ToList()
        };
    }

    private static RoundItemResponse MapToRoundItemResponse(RoundItem item)
    {
        return new RoundItemResponse
        {
            Id = item.Id,
            Title = item.Title,
            Artist = item.Artist,
            Points = item.Points,
            ExtraInfo = item.ExtraInfo,
            OrderIndex = item.OrderIndex,
            IsAnswerRevealed = item.IsAnswerRevealed,
            SpotifyId = item.SpotifyId,
            Year = item.Year
        };
    }
} 