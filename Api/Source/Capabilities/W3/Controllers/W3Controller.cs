using Microsoft.AspNetCore.Mvc;
using Api.W3.Models.Requests;

namespace Api.W3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class W3Controller : ControllerBase
{
    private readonly IW3Service _w3Service;
    private readonly ILogger<W3Controller> _logger;

    public W3Controller(
        IW3Service w3Service,
        ILogger<W3Controller> logger)
    {
        _w3Service = w3Service;
        _logger = logger;
    }

    [HttpPost("validate")]
    public async Task<ActionResult<W3ValidationResponse>> ValidateHtml([FromBody] ValidateHtmlRequest request)
    {
        try
        {
            var result = await _w3Service.ValidateHtmlAsync(request.Html);
            _logger.LogInformation("W3 validation result: {@Result}", result);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate HTML");
            return StatusCode(500, "Failed to validate HTML");
        }
    }
} 