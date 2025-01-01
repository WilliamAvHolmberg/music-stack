using Microsoft.EntityFrameworkCore;
using Api.Domain.Games.Models;
using Api.Shared.Infrastructure.Database;

namespace Api.Domain.Games.Repositories;

public class GameTemplateRepository : IGameTemplateRepository
{
    private readonly AppDbContext _context;

    public GameTemplateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GameTemplate>> GetAllAsync()
    {
        return await _context.GameTemplates
            .Include(t => t.Rounds)
                .ThenInclude(r => r.Items)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<GameTemplate?> GetByIdAsync(int id)
    {
        return await _context.GameTemplates
            .Include(t => t.Rounds)
                .ThenInclude(r => r.Items)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<GameTemplate> UpsertAsync(GameTemplate template)
    {
        if (template.Id == 0)
        {
            _context.GameTemplates.Add(template);
        }
        else
        {
            // Delete existing rounds and their items
            var existingRounds = await _context.Rounds
                .Include(r => r.Items)
                .Where(r => r.GameTemplateId == template.Id)
                .ToListAsync();

            foreach (var round in existingRounds)
            {
                _context.RoundItems.RemoveRange(round.Items);
                _context.Rounds.Remove(round);
            }

            _context.GameTemplates.Update(template);
        }

        await _context.SaveChangesAsync();
        return template;
    }

    public async Task DeleteAsync(int id)
    {
        var template = await _context.GameTemplates.FindAsync(id);
        if (template != null)
        {
            _context.GameTemplates.Remove(template);
            await _context.SaveChangesAsync();
        }
    }
} 