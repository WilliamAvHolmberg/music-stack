using Microsoft.Extensions.Configuration;
using System.IO;

namespace Api.Configuration;

public static class EnvironmentConfiguration
{
    public static IConfigurationBuilder AddEnvironmentFile(this IConfigurationBuilder builder)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        // In development, try to load .env from solution root
        if (environment != "Production")
        {
            var envPath = FindEnvFile();
            if (envPath != null)
            {
                LoadEnvFile(envPath);
            }
        }
        
        return builder.AddEnvironmentVariables();
    }
    
    public static AppSettings GetValidatedSettings(this IConfiguration configuration)
    {
        try
        {
            var settings = new AppSettings
            {
                ApiKeys = new ApiKeys
                {
                    OpenAiKey = GetRequiredValue("OPENAI_API_KEY"),
                    ClaudeKey = GetRequiredValue("CLAUDE_API_KEY")
                },
                Sms = new SmsSettings
                {
                    ElksApiUsername = GetRequiredValue("ELKS_API_USERNAME"),
                    ElksApiPassword = GetRequiredValue("ELKS_API_PASSWORD")
                },
                Database = new DatabaseSettings()
            };

            return settings;

            string GetRequiredValue(string key)
            {
                var value = Environment.GetEnvironmentVariable(key);
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException($"Required environment variable '{key}' is not set");
                }
                return value;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to load required environment variables. Please check your .env file or environment variables.", ex);
        }
    }
    
    private static string? FindEnvFile()
    {
        var directory = Directory.GetCurrentDirectory();
        while (directory != null)
        {
            var envPath = Path.Combine(directory, ".env");
            if (File.Exists(envPath))
            {
                return envPath;
            }
            directory = Directory.GetParent(directory)?.FullName;
        }
        return null;
    }
    
    private static void LoadEnvFile(string path)
    {
        foreach (var line in File.ReadAllLines(path))
        {
            var trimmedLine = line.Trim();
            
            // Skip empty lines and comments
            if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith("#"))
                continue;
                
            var parts = trimmedLine.Split('=', 2);
            if (parts.Length != 2) continue;
            
            var key = parts[0].Trim();
            var value = parts[1].Trim();
            
            // Remove quotes if present
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Substring(1, value.Length - 2);
            }
            
            // Don't override existing environment variables
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
} 