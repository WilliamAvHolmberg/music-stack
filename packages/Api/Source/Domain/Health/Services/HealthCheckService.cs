namespace Api.AI;

public interface IHealthCheckService
{
    Task<HealthStatus> GetHealthStatusAsync();
}

public class HealthCheckService : IHealthCheckService
{
    private readonly ILogger<HealthCheckService> _logger;
    private readonly ISystemInfoService _systemInfoService;

    public HealthCheckService(
        ILogger<HealthCheckService> logger,
        ISystemInfoService systemInfoService)
    {
        _logger = logger;
        _systemInfoService = systemInfoService;
    }

    public async Task<HealthStatus> GetHealthStatusAsync()
    {
        try
        {
            var systemInfo = await _systemInfoService.GetSystemInfoAsync();

            return new HealthStatus
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = systemInfo.Version,
                Environment = systemInfo.Environment
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking health status");
            throw;
        }
    }
}

public class HealthStatus
{
    public required string Status { get; set; }
    public DateTime Timestamp { get; set; }
    public required string Version { get; set; }
    public required string Environment { get; set; }
}
