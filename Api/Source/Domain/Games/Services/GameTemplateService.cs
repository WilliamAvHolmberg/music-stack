using Api.Domain.Games.Models;
using Api.Domain.Games.Models.Requests;
using Api.Domain.Games.Models.Responses;
using Api.Shared.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Games.Services;

public class GameTemplateService : IGameTemplateService
{
    private readonly AppDbContext _context;
    private readonly ILogger<GameTemplateService> _logger;

    public GameTemplateService(AppDbContext context, ILogger<GameTemplateService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<GameTemplateResponse>> GetTemplatesAsync()
    {
        var templates = await _context.GameTemplates
            .Include(t => t.Rounds)
                .ThenInclude(r => r.Items)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
            
        return templates.Select(MapToResponse);
    }

    public async Task<GameTemplateResponse?> GetTemplateAsync(int id)
    {
        var template = await _context.GameTemplates
            .Include(t => t.Rounds)
                .ThenInclude(r => r.Items)
            .FirstOrDefaultAsync(t => t.Id == id);
            
        return template != null ? MapToResponse(template) : null;
    }

    public async Task<GameTemplateResponse> UpsertTemplateAsync(int? id, CreateGameTemplateRequest request)
    {
        var template = id.HasValue 
            ? await _context.GameTemplates
                .Include(t => t.Rounds)
                    .ThenInclude(r => r.Items)
                .FirstOrDefaultAsync(t => t.Id == id.Value)
            : new GameTemplate
            {
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                Rounds = new List<Round>()
            };

        if (template == null)
        {
            throw new KeyNotFoundException($"Template {id} not found");
        }

        // Update basic properties
        template.Name = request.Name;
        template.Description = request.Description;
        template.IsPublic = request.IsPublic;

        if (id.HasValue)
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
        }

        // Create rounds and items
        template.Rounds = request.Rounds.Select((r, index) => new Round
        {
            GameTemplateId = template.Id,
            Title = r.Title,
            Type = r.Type,
            TimeInMinutes = r.TimeInMinutes,
            Instructions = r.Instructions,
            OrderIndex = index,
            Status = RoundStatus.NotStarted,
            Items = r.Items.Select((i, itemIndex) => new RoundItem
            {
                Title = i.Title,
                Artist = i.Artist,
                Points = i.Points,
                ExtraInfo = i.ExtraInfo,
                SpotifyId = i.SpotifyId,
                OrderIndex = itemIndex
            }).ToList()
        }).ToList();

        // Save changes
        if (id.HasValue)
        {
            _context.GameTemplates.Update(template);
        }
        else
        {
            _context.GameTemplates.Add(template);
        }
        await _context.SaveChangesAsync();

        // Return updated template
        var savedTemplate = await _context.GameTemplates
            .Include(t => t.Rounds)
                .ThenInclude(r => r.Items)
            .FirstOrDefaultAsync(t => t.Id == template.Id);
            
        return MapToResponse(savedTemplate!);
    }

    public async Task DeleteTemplateAsync(int id)
    {
        var template = await _context.GameTemplates.FindAsync(id);
        if (template != null)
        {
            _context.GameTemplates.Remove(template);
            await _context.SaveChangesAsync();
        }
    }

    private static GameTemplateResponse MapToResponse(GameTemplate template) => new()
    {
        Id = template.Id,
        Name = template.Name,
        Description = template.Description,
        CreatedAt = template.CreatedAt,
        IsPublic = template.IsPublic,
        Rounds = template.Rounds
            .OrderBy(r => r.OrderIndex)
            .Select(r => new RoundResponse
            {
                Id = r.Id,
                Title = r.Title,
                Type = r.Type,
                Status = r.Status,
                TimeInMinutes = r.TimeInMinutes,
                Instructions = r.Instructions,
                OrderIndex = r.OrderIndex,
                Items = r.Items
                    .OrderBy(i => i.OrderIndex)
                    .Select(i => new RoundItemResponse
                    {
                        Id = i.Id,
                        Title = i.Title,
                        Artist = i.Artist,
                        Points = i.Points,
                        ExtraInfo = i.ExtraInfo,
                        SpotifyId = i.SpotifyId,
                        OrderIndex = i.OrderIndex,
                        Year = i.Year
                    }).ToList()
            }).ToList()
    };

    private static RoundItemResponse MapToTemplateItemResponse(RoundItem item)
    {
        return new RoundItemResponse
        {
            Id = item.Id,
            Title = item.Title,
            Artist = item.Artist,
            Points = item.Points,
            ExtraInfo = item.ExtraInfo,
            OrderIndex = item.OrderIndex,
            IsAnswerRevealed = false,
            SpotifyId = item.SpotifyId,
            Year = item.Year
        };
    }
} 