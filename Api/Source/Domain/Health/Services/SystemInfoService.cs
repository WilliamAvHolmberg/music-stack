namespace Api.AI;

public interface ISystemInfoService
{
    Task<SystemInfo> GetSystemInfoAsync();
}

public class SystemInfoService : ISystemInfoService
{
    private readonly IConfiguration _configuration;

    public SystemInfoService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<SystemInfo> GetSystemInfoAsync()
    {
        var systemInfo = new SystemInfo
        {
            Version = _configuration["AppSettings:Version"] ?? "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
            OsVersion = Environment.OSVersion.ToString(),
            MachineName = Environment.MachineName
        };

        return Task.FromResult(systemInfo);
    }
}

public class SystemInfo
{
    public required string Version { get; set; }
    public required string Environment { get; set; }
    public required string OsVersion { get; set; }
    public required string MachineName { get; set; }
}