using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Api.Domain.Games.Hubs;

public class GameHub : Hub
{
    private readonly ILogger<GameHub> _logger;

    public GameHub(ILogger<GameHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation(
            "Client connected: {ConnectionId} from {IPAddress} with protocol {Protocol}", 
            Context.ConnectionId,
            Context.GetHttpContext()?.Connection.RemoteIpAddress,
            Context.GetHttpContext()?.Request.Protocol
        );
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        if (exception != null)
        {
            _logger.LogError(exception, "Client disconnected with error");
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinGame(int gameId)
    {
        _logger.LogInformation(
            "Client {ConnectionId} joining game {GameId} from {IPAddress}", 
            Context.ConnectionId,
            gameId,
            Context.GetHttpContext()?.Connection.RemoteIpAddress
        );
        await Groups.AddToGroupAsync(Context.ConnectionId, $"game_{gameId}");
        _logger.LogInformation("Client {ConnectionId} successfully joined game {GameId}", Context.ConnectionId, gameId);
    }

    public async Task LeaveGame(int gameId)
    {
        _logger.LogInformation(
            "Client {ConnectionId} leaving game {GameId}", 
            Context.ConnectionId,
            gameId
        );
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"game_{gameId}");
        _logger.LogInformation("Client {ConnectionId} successfully left game {GameId}", Context.ConnectionId, gameId);
    }
} 