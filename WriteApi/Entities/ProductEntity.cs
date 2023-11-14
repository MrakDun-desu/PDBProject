namespace WriteApi.Entities;

public record ProductEntity
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required float Price { get; set; }
    public required uint StockCount { get; set; }
    public string? Description { get; set; }

    public string[] Categories { get; set; } = Array.Empty<string>();
}