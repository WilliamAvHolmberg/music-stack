using Api.Domain.Games.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Api.Domain.Games.Services;

public interface IGameNotificationService
{
    Task NotifyGameChanged(int gameId);
}

public class GameNotificationService : IGameNotificationService
{
    private readonly IHubContext<GameHub> _hubContext;
    private readonly ILogger<GameNotificationService> _logger;

    public GameNotificationService(IHubContext<GameHub> hubContext, ILogger<GameNotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task NotifyGameChanged(int gameId)
    {
        try
        {
            _logger.LogInformation("Sending GameChanged notification for game {GameId}", gameId);
            await _hubContext.Clients.Group($"game_{gameId}")
                .SendAsync("GameChanged", gameId);
            _logger.LogInformation("Successfully sent GameChanged notification for game {GameId}", gameId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to notify game change for game {GameId}", gameId);
        }
    }
} 