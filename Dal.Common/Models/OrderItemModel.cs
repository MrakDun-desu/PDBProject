namespace PDBProject.Dal.Common.Models;

/// <summary>
/// Represents one item in an order.
/// </summary>
public class OrderItemModel
{
    public required int OrderId { get; init; }
    public required int ProductId { get; init; }
    public required uint ProductCount { get; set; }
}