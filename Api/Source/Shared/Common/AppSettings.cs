namespace Api.Configuration;
using System.IO;

public class AppSettings
{
    public required ApiKeys ApiKeys { get; set; }
    public required SmsSettings Sms { get; set; }
    public required DatabaseSettings Database { get; set; }
}

public class ApiKeys
{
    public required string OpenAiKey { get; set; }
    public required string ClaudeKey { get; set; }
}

public class SmsSettings
{
    public required string ElksApiUsername { get; set; }
    public required string ElksApiPassword { get; set; }
}

public class DatabaseSettings
{
    private string sqlitePath = "data/app.db";
    
    public string SqlitePath
    {
        get => sqlitePath;
        set => sqlitePath = value;
    }
} 