using Microsoft.EntityFrameworkCore;

namespace WriteApi.Entities;

[PrimaryKey(nameof(ProductId), nameof(OrderId))]
public record OrderItemEntity
{
    public required int ProductId { get; init; }
    public ProductEntity Product { get; init; } = null!;
    public required int OrderId { get; init; }
    public OrderEntity Order { get; init; } = null!;
    public required uint ProductCount { get; set; }
}