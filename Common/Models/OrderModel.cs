using PDBProject.Common.Enums;

namespace PDBProject.Common.Models;

public record OrderModel
{
    public int Id { get; init; }
    public required int UserId { get; init; }
    public OrderState State { get; set; } = OrderState.InBasket;
    public DateTime? OrderedDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
}