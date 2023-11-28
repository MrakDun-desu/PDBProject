using PDBProject.Common.Enums;

namespace PDBProject.ReadApi.Models;

public class OrderEntity
{
    public OrderState State { get; set; } = OrderState.InBasket;

    public DateTime? OrderedDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public List<ProductListEntity> Products { get; set; } = null!;
}