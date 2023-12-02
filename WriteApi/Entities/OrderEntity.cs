using PDBProject.Dal.Common.Enums;

namespace PDBProject.WriteApi.Entities;

/// <summary>
/// Postgres representation of the OrderModel.
/// </summary>
public record OrderEntity
{
    public int Id { get; init; }
    public required int UserId { get; init; }
    public UserEntity User { get; init; } = null!;
    public OrderState State { get; set; } = OrderState.InBasket;
    public DateTime? OrderedDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public ICollection<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
}