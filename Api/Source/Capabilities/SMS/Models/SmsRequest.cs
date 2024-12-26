namespace Api.SMS;

public class SmsRequest
{
    public required string From { get; set; }
    public required string To { get; set; }
    public required string Message { get; set; }
} 