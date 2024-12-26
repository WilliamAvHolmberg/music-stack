# Backend Conventions and Structure

## Project Structure

```
Api/
├── Source/
│   ├── Domain/           # Business domain features
│   │   ├── Flashcards/  # Example domain
│   │   │   ├── Controllers/
│   │   │   ├── Models/
│   │   │   │   ├── Requests/    # DTOs for incoming requests
│   │   │   │   ├── Responses/   # DTOs for outgoing responses
│   │   │   │   └── *.cs         # Domain models
│   │   │   └── Services/
│   │   └── ...
│   ├── Capabilities/     # Cross-cutting technical capabilities
│   │   ├── AI/          # AI integration capability
│   │   ├── W3/          # W3C validation capability
│   │   └── SMS/         # SMS capability
│   └── Shared/          # Shared infrastructure and utilities
│       ├── Infrastructure/
│       │   ├── Database/
│       │   ├── Logging/
│       │   └── Security/
│       └── Utils/
└── Tests/               # Test projects mirror the Source structure
```

## Naming Conventions

### Files and Directories
- Use PascalCase for all files and directories
- Controllers: `{Feature}Controller.cs`
- Models: Singular form, e.g., `Flashcard.cs`
- Interfaces: Prefix with 'I', e.g., `IFlashcardService.cs`
- Request DTOs: Suffix with 'Request', e.g., `GenerateFlashcardsRequest.cs`
- Response DTOs: Suffix with 'Response', e.g., `FlashcardGenerationResponse.cs`

### Namespaces
- Root namespace: `Api`
- Domain features: `Api.{Feature}`, e.g., `Api.Flashcards`
- Capabilities: `Api.{Capability}`, e.g., `Api.AI`
- Shared code: `Api.Shared.{Category}`, e.g., `Api.Shared.Infrastructure`

## Domain Organization

### Controllers
- One controller per domain feature
- Route pattern: `api/[controller]`
- Use attribute routing
- Return ActionResult<T>
- Include try-catch with logging
- Use proper HTTP methods and status codes

```csharp
[ApiController]
[Route("api/[controller]")]
public class FlashcardsController : ControllerBase
{
    private readonly IFlashcardService _service;
    private readonly ILogger<FlashcardsController> _logger;

    [HttpPost]
    public async Task<ActionResult<T>> Method()
    {
        try
        {
            // Implementation
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error message");
            return StatusCode(500, "User-friendly message");
        }
    }
}
```

### Models
- Use records for DTOs (immutable data)
- Use classes for domain models
- Required properties use `required` keyword
- Use nullable reference types
- Include validation attributes
- Use JsonIgnore for navigation properties

```csharp
public class DomainModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    
    [JsonIgnore]
    public RelatedEntity? Related { get; set; }
}

public record RequestDto(
    [Required] string Title,
    string? Description
);
```

### Services
- Interface-based design
- Dependency injection
- Async by default
- Business logic only

```csharp
public interface IService
{
    Task<T> MethodAsync(int id);
}

public class Service : IService
{
    private readonly AppDbContext _context;
    private readonly ILogger<Service> _logger;

    public Service(AppDbContext context, ILogger<Service> logger)
    {
        _context = context;
        _logger = logger;
    }
}
```

## Database Conventions

### Entity Framework
- Use fluent configuration in AppDbContext
- Define relationships explicitly
- Use proper delete behaviors
- Index performance-critical queries

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Entity>(entity =>
    {
        entity.HasMany(e => e.Children)
            .WithOne(e => e.Parent)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => e.SearchableColumn);
    });
}
```

## Error Handling
- Use custom exception types for domain-specific errors
- Global exception handling middleware
- Consistent error response format
- Proper logging with context

```csharp
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

// Error Response Format
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "Error title",
    "status": 400,
    "detail": "Detailed error message",
    "traceId": "request-trace-id"
}
```

## Capabilities vs Domain Features

### Domain Features
- Business logic specific to a domain
- Located in `Source/Domain/{Feature}`
- Examples: Flashcards, Reviews, Study

### Capabilities
- Technical features used across domains
- Located in `Source/Capabilities/{Capability}`
- Independent of business logic
- Examples: AI integration, SMS, W3C validation

## Testing Conventions
- Tests mirror the source structure
- One test class per production class
- Use descriptive test names
- Arrange-Act-Assert pattern
- Use test data builders
- Mock external dependencies

```csharp
[Fact]
public async Task MethodName_Scenario_ExpectedBehavior()
{
    // Arrange
    var dependency = new Mock<IDependency>();
    var sut = new SystemUnderTest(dependency.Object);

    // Act
    var result = await sut.MethodAsync();

    // Assert
    result.Should().NotBeNull();
}
```

## API Response Conventions
- Use consistent response envelopes
- Include pagination for collections
- Use proper HTTP status codes
- Include proper headers (e.g., caching, CORS)

```json
// Collection Response
{
    "items": [],
    "totalCount": 0,
    "pageSize": 20,
    "currentPage": 1,
    "totalPages": 1
}

// Single Item Response
{
    "data": {},
    "links": {
        "self": "/api/resource/1",
        "related": "/api/resource/1/related"
    }
}
```

## Security Conventions
- Use HTTPS everywhere
- Implement proper authentication
- Use authorization attributes
- Validate all inputs
- Sanitize all outputs
- Use secure headers
- Follow OWASP guidelines

## Performance Guidelines
- Use async/await consistently
- Implement caching where appropriate
- Use proper indexing
- Optimize database queries
- Use pagination for large datasets
- Profile and monitor performance

This document should be treated as a living standard. When new patterns emerge or better practices are identified, this document should be updated to reflect those improvements. 