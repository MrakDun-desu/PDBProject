namespace WriteApi.Entities;

public record CategoryEntity
{
    public int Id { get; init; }
    public required string Name { get; set; }

    public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}