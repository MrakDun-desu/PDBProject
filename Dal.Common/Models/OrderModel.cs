using PDBProject.Dal.Common.Enums;

namespace PDBProject.Dal.Common.Models;

/// <summary>
/// Represents a whole order with its state, corresponding dates and ID of the user that has ordered it.
/// </summary>
public record OrderModel
{
    public int Id { get; init; }
    public required int UserId { get; init; }
    public OrderState State { get; set; } = OrderState.InBasket;
    public DateTime? OrderedDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
}