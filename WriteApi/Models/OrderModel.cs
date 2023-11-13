namespace WriteApi.Models;

public record OrderModel
{
    public required int Id { get; init; }
    public required int UserId { get; init; }
    public DateOnly? Date { get; set; }
    
    public ICollection<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
}