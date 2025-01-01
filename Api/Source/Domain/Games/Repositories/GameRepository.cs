using Api.Domain.Games.Models;
using Api.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Api.Shared.Infrastructure.Database;
using Api.Shared.Infrastructure.Exceptions;

namespace Api.Domain.Games.Repositories;

public class GameRepository : IGameRepository
{
    private readonly AppDbContext _context;

    public GameRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GameSession?> GetByIdAsync(int id)
    {
        return await _context.GameSessions
            .Include(g => g.Teams)
            .Include(g => g.Rounds)
                .ThenInclude(r => r.Items)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<GameSession>> GetActiveGamesAsync()
    {
        return await _context.GameSessions
            .Include(g => g.Teams)
            .Where(g => g.Status != GameStatus.Finished)
            .ToListAsync();
    }

    public async Task<GameSession> CreateGameAsync(GameSession game)
    {
        _context.GameSessions.Add(game);
        await _context.SaveChangesAsync();
        return game;
    }

    public async Task UpdateGameAsync(GameSession game)
    {
        _context.Entry(game).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteGameAsync(int id)
    {
        var game = await GetByIdAsync(id);
        if (game != null)
        {
            _context.GameSessions.Remove(game);
            await _context.SaveChangesAsync();
        }
    }

    // Team operations
    public async Task<Team> AddTeamAsync(Team team)
    {
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();
        return team;
    }

    public async Task UpdateTeamAsync(Team team)
    {
        _context.Entry(team).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Team>> GetTeamsByGameIdAsync(int gameId)
    {
        return await _context.Teams
            .Where(t => t.GameSessionId == gameId)
            .ToListAsync();
    }

    // Round operations
    public async Task<Round> AddRoundAsync(Round round)
    {
        _context.Rounds.Add(round);
        await _context.SaveChangesAsync();
        return round;
    }

    public async Task UpdateRoundAsync(Round round)
    {
        _context.Entry(round).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Round>> GetRoundsByGameIdAsync(int gameId)
    {
        return await _context.Rounds
            .Include(r => r.CurrentSong)
            .Where(r => r.GameSessionId == gameId)
            .ToListAsync();
    }

    public async Task<Round?> GetCurrentRoundAsync(int gameId)
    {
        return await _context.Rounds
            .Include(r => r.CurrentSong)
            .Where(r => r.GameSessionId == gameId && r.Status == RoundStatus.InProgress)
            .FirstOrDefaultAsync();
    }

    public async Task<RoundItem> AddRoundItemAsync(RoundItem item)
    {
        _context.RoundItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<GameTemplate?> GetTemplateAsync(int id)
    {
        return await _context.GameTemplates
            .Include(t => t.Rounds)
            .ThenInclude(r => r.Items)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task RevealAnswerAsync(int gameId, int itemId)
    {
        var currentRound = await GetCurrentRoundAsync(gameId);
        if (currentRound == null)
        {
            throw new NotFoundException("No active round found");
        }

        var item = currentRound.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
        {
            throw new NotFoundException($"Item {itemId} not found in current round");
        }

        item.IsAnswerRevealed = true;
        await _context.SaveChangesAsync();
    }
} 