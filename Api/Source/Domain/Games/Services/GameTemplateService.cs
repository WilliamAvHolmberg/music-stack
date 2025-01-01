using Api.Domain.Games.Models;
using Api.Domain.Games.Models.Requests;
using Api.Domain.Games.Models.Responses;
using Api.Domain.Games.Repositories;

namespace Api.Domain.Games.Services;

public class GameTemplateService : IGameTemplateService
{
    private readonly IGameTemplateRepository _repository;
    private readonly ILogger<GameTemplateService> _logger;

    public GameTemplateService(IGameTemplateRepository repository, ILogger<GameTemplateService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<GameTemplateResponse>> GetTemplatesAsync()
    {
        var templates = await _repository.GetAllAsync();
        return templates.Select(MapToResponse);
    }

    public async Task<GameTemplateResponse?> GetTemplateAsync(int id)
    {
        var template = await _repository.GetByIdAsync(id);
        return template != null ? MapToResponse(template) : null;
    }

    public async Task<GameTemplateResponse> UpsertTemplateAsync(int? id, CreateGameTemplateRequest request)
    {
        var template = id.HasValue 
            ? await _repository.GetByIdAsync(id.Value) 
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
        await _repository.UpsertAsync(template);

        // Return updated template
        var savedTemplate = await _repository.GetByIdAsync(template.Id);
        return MapToResponse(savedTemplate!);
    }

    public async Task DeleteTemplateAsync(int id)
    {
        await _repository.DeleteAsync(id);
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