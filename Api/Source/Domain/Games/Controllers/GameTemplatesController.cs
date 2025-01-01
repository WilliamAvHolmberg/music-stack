using Microsoft.AspNetCore.Mvc;
using Api.Domain.Games.Models;
using Api.Domain.Games.Models.Requests;
using Api.Domain.Games.Models.Responses;
using Api.Domain.Games.Services;

namespace Api.Domain.Games.Controllers;

[ApiController]
[Route("api/templates")]
public class GameTemplatesController : ControllerBase
{
    private readonly IGameTemplateService _service;
    private readonly ILogger<GameTemplatesController> _logger;

    public GameTemplatesController(IGameTemplateService service, ILogger<GameTemplatesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameTemplateResponse>>> GetTemplates()
    {
        try
        {
            var templates = await _service.GetTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting templates");
            return StatusCode(500, "An error occurred while getting templates");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameTemplateResponse>> GetTemplate(int id)
    {
        try
        {
            var template = await _service.GetTemplateAsync(id);
            if (template == null)
            {
                return NotFound();
            }
            return Ok(template);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting template {Id}", id);
            return StatusCode(500, "An error occurred while getting the template");
        }
    }

    [HttpPost]
    public async Task<ActionResult<GameTemplateResponse>> CreateTemplate(CreateGameTemplateRequest request)
    {
        try
        {
            var template = await _service.UpsertTemplateAsync(null, request);
            return CreatedAtAction(nameof(GetTemplate), new { id = template.Id }, template);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating template");
            return StatusCode(500, "An error occurred while creating the template");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GameTemplateResponse>> UpdateTemplate(int id, CreateGameTemplateRequest request)
    {
        try
        {
            var template = await _service.UpsertTemplateAsync(id, request);
            return Ok(template);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating template {Id}", id);
            return StatusCode(500, "An error occurred while updating the template");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTemplate(int id)
    {
        try
        {
            await _service.DeleteTemplateAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting template {Id}", id);
            return StatusCode(500, "An error occurred while deleting the template");
        }
    }
} 