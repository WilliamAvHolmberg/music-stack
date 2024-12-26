namespace Api.SMS;

public class ElksSmsRequest
{
    public required string from { get; set; }
    public required string to { get; set; }
    public required string message { get; set; }
} 