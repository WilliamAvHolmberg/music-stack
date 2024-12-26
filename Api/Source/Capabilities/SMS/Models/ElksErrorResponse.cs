namespace Api.SMS;

public class ElksErrorResponse
{
    public ElksError? error { get; set; }
}

public class ElksError
{
    public string? code { get; set; }
    public string? message { get; set; }
} 