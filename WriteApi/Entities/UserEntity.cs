namespace WriteApi.Entities;

public record UserEntity
{
    public required int Id { get; init; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
}