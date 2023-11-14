namespace WriteApi.Entities;

public record OrderEntity
{
    public int Id { get; init; }
    public required int UserId { get; init; }
    public required UserEntity User { get; init; }
    public DateOnly? Date { get; set; }

    public ICollection<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
}