using MongoDB.Bson.Serialization.Attributes;

namespace PDBProject.ReadApi.Models;

public class ProductEntity
{
    [BsonId] public required int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<string> Categories { get; set; } = new List<string>();

    public int Price { get; set; }

    public string? Description { get; set; }

    public int StockCount { get; set; }
}