namespace PDBProject.Dal.Common.Enums;

public enum OrderState
{
    /// <summary>
    /// All items in the order at in the basket, they can still change in any way.
    /// </summary>
    InBasket = 0,
    /// <summary>
    /// The customer has finished the order. Items in the basket cannot be changed anymore.
    /// </summary>
    Ordered = 1,
    /// <summary>
    /// Order has been shipped, now the customer is waiting to receive the order.
    /// </summary>
    Shipped = 2,
    /// <summary>
    /// Order has been successfully received and there is nothing more that can change about it.
    /// </summary>
    Received = 3
}