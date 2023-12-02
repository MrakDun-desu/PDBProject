namespace PDBProject.Dal.Common.Models;

/// <summary>
/// Represents a product in a shop. Has a required name, price, stock count and categories that it belongs to.
/// Can also have a description.
/// </summary>
public record ProductModel
{
    public required int Id { get; init; }
    public required string Name { get; set; }
    public required float Price { get; set; }
    public required uint StockCount { get; set; }
    public string? Description { get; set; }
    public ICollection<string> Categories { get; set; } = new List<string>();
}