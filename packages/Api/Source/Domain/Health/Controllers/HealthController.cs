using Microsoft.AspNetCore.Mvc;
using Api.AI;

namespace Api.Health;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(
        ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<HealthStatus>> GetHealth()
    {
        try
        {
            return Ok("OK");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting health status");
            return StatusCode(500, new { message = "Error checking health status" });
        }
    }
}