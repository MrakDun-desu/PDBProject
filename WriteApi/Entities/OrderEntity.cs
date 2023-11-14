using WriteApi.Enums;

namespace WriteApi.Entities;

public record OrderEntity
{
    public int Id { get; init; }
    public required int UserId { get; init; }
    public UserEntity User { get; init; } = null!;
    public OrderState State { get; set; } = OrderState.InBasket;
    public DateOnly? OrderedDate { get; set; }
    public DateOnly? ShippedDate { get; set; }
    public DateOnly? ReceivedDate { get; set; }

    public ICollection<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
}