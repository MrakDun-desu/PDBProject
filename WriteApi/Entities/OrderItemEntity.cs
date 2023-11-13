namespace WriteApi.Entities;

public record OrderItemEntity
{
    public required int Id { get; init; }
    public required int ProductId { get; init; }
    public required ProductEntity Product { get; init; }
    public required int OrderId { get; init; }
    public required OrderEntity Order { get; init; }
    public required uint ProductCount { get; set; }
}