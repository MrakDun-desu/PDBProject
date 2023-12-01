namespace PDBProject.Dal.Common.Models;

public record ProductModel
{
    public required int Id { get; init; }
    public required string Name { get; set; }
    public required float Price { get; set; }
    public required uint StockCount { get; set; }
    public string? Description { get; set; }
    public ICollection<string> Categories { get; set; } = new List<string>();
}