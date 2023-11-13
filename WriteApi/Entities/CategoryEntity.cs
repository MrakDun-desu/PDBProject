namespace WriteApi.Entities;

public record CategoryEntity
{
    public required int Id { get; init; }
    public required string CategoryName { get; set; }

    public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}