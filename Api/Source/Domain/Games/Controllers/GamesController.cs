using Microsoft.AspNetCore.Mvc;
using Api.Domain.Games.Models;
using Api.Domain.Games.Models.Requests;
using Api.Domain.Games.Models.Responses;
using Api.Domain.Games.Services;
using Api.Shared.Infrastructure.Exceptions;

namespace Api.Domain.Games.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly ILogger<GamesController> _logger;

    public GamesController(IGameService gameService, ILogger<GamesController> logger)
    {
        _gameService = gameService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<GameSessionResponse>> CreateGame(CreateGameRequest request)
    {
        try
        {
            var game = await _gameService.CreateGameAsync(request.GameTemplateId);
            return Ok(game);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating game");
            return StatusCode(500, "An error occurred while creating the game");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameSessionResponse>> GetGame(int id)
    {
        try
        {
            var game = await _gameService.GetGameAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting game {Id}", id);
            return StatusCode(500, "An error occurred while getting the game");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameSessionResponse>>> GetActiveGames()
    {
        try
        {
            var games = await _gameService.GetActiveGamesAsync();
            return Ok(games);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active games");
            return StatusCode(500, "An error occurred while getting active games");
        }
    }

    [HttpPost("{gameId}/teams")]
    public async Task<ActionResult<TeamResponse>> AddTeam(int gameId, CreateTeamRequest request)
    {
        try
        {
            var team = await _gameService.AddTeamAsync(gameId, request);
            return Ok(team);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding team to game {GameId}", gameId);
            return StatusCode(500, "An error occurred while adding the team");
        }
    }

    [HttpPost("{gameId}/rounds")]
    public async Task<ActionResult<RoundResponse>> StartRound(int gameId, CreateRoundRequest request)
    {
        try
        {
            var round = await _gameService.StartRoundAsync(gameId, request);
            return Ok(round);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting round in game {GameId}", gameId);
            return StatusCode(500, "An error occurred while starting the round");
        }
    }

    [HttpPost("{gameId}/rounds/end")]
    public async Task<ActionResult<RoundResponse>> EndRound(int gameId)
    {
        try
        {
            var round = await _gameService.EndRoundAsync(gameId);
            return Ok(round);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ending round in game {GameId}", gameId);
            return StatusCode(500, "An error occurred while ending the round");
        }
    }

    [HttpPost("{id}/end")]
    public async Task<ActionResult<GameSessionResponse>> EndGame(int id)
    {
        try
        {
            var game = await _gameService.EndGameAsync(id);
            return Ok(game);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ending game {Id}", id);
            return StatusCode(500, "An error occurred while ending the game");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGame(int id)
    {
        try
        {
            await _gameService.DeleteGameAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting game {Id}", id);
            return StatusCode(500, "An error occurred while deleting the game");
        }
    }

    [HttpPut("{gameId}/teams/{teamId}/score")]
    public async Task<ActionResult<TeamResponse>> UpdateTeamScore(int gameId, int teamId, [FromBody] int score)
    {
        try
        {
            var team = await _gameService.UpdateTeamScoreAsync(gameId, teamId, score);
            return Ok(team);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating team {TeamId} score in game {GameId}", teamId, gameId);
            return StatusCode(500, "An error occurred while updating the team score");
        }
    }

    [HttpPost("{id}/start")]
    public async Task<ActionResult<GameSessionResponse>> StartGame(int id)
    {
        try
        {
            var game = await _gameService.StartGameAsync(id);
            return Ok(game);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting game {Id}", id);
            return StatusCode(500, "An error occurred while starting the game");
        }
    }

    [HttpPut("{gameId}/items/next")]
    public async Task<ActionResult<GameSessionResponse>> NextItem(int gameId)
    {
        try
        {
            var game = await _gameService.NextItemAsync(gameId);
            return Ok(game);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{gameId}/items/previous")]
    public async Task<ActionResult<GameSessionResponse>> PreviousItem(int gameId)
    {
        try
        {
            var game = await _gameService.PreviousItemAsync(gameId);
            return Ok(game);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{gameId}/timer/pause")]
    public async Task<ActionResult<GameSessionResponse>> PauseTimer(int gameId)
    {
        try
        {
            var game = await _gameService.PauseTimerAsync(gameId);
            return Ok(game);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{gameId}/timer/resume")]
    public async Task<ActionResult<GameSessionResponse>> ResumeTimer(int gameId)
    {
        try
        {
            var game = await _gameService.ResumeTimerAsync(gameId);
            return Ok(game);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{gameId}/timer/reset")]
    public async Task<ActionResult<GameSessionResponse>> ResetTimer(int gameId)
    {
        try
        {
            var game = await _gameService.ResetTimerAsync(gameId);
            return Ok(game);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{gameId}/state")]
    public async Task<ActionResult<GameSessionResponse>> UpdateGameState(int gameId, [FromBody] UpdateGameStateRequest request)
    {
        try
        {
            var game = await _gameService.UpdateGameStateAsync(gameId, request);
            return Ok(game);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{gameId}/items/{itemId}/reveal/toggle")]
    public async Task<ActionResult> ToggleItemReveal(int gameId, int itemId)
    {
        try 
        {
            await _gameService.RevealAnswerAsync(gameId, itemId);
            return Ok();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling item reveal in game {GameId}, item {ItemId}", gameId, itemId);
            return StatusCode(500, "An error occurred while toggling the item reveal");
        }
    }
} 