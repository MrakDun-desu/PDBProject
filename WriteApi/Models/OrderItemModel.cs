namespace WriteApi.Models;

public class OrderItemModel
{
    public required int Id { get; init; }
    public required int ProductId { get; init; }
    public required int OrderId { get; init; }
    public required uint ProductCount { get; set; }
}