namespace PDBProject.Common.Models;

public class OrderItemModel
{
    public required int OrderId { get; init; }
    public required int ProductId { get; init; }
    public required uint ProductCount { get; set; }
}