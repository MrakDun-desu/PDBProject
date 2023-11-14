using WriteApi.Enums;

namespace WriteApi.Models;

public record OrderModel
{
    public int Id { get; init; }
    public required int UserId { get; init; }
    public OrderState State { get; set; } = OrderState.InBasket;
    public DateOnly? OrderedDate { get; set; }
    public DateOnly? ShippedDate { get; set; }
    public DateOnly? ReceivedDate { get; set; }
}