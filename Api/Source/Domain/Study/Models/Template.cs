public class Template
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    // Variables are stored in Content using {{variable}} format
    // e.g., "Hello {{name}}, your order {{orderId}} is ready"
} 