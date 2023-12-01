using PDBProject.Dal.Common.Enums;

namespace PDBProject.Dal.Mongo.Entities;

public class OrderEntity
{
    public int Id { get; init; }
    public OrderState State { get; set; } = OrderState.InBasket;
    public DateTime? OrderedDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public List<OrderItemEntity> OrderItems { get; set; } = null!;
}