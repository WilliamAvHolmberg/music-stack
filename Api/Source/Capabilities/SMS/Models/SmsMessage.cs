namespace Api.SMS;

public class SmsMessage
{
    public int Id { get; set; }
    public required string From { get; set; }
    public required string To { get; set; }
    public required string Message { get; set; }
    public required string Status { get; set; }
    public string? ElksMessageId { get; set; }
    public DateTime CreatedAt { get; set; }
} 