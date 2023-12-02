using MongoDB.Bson.Serialization.Attributes;

namespace PDBProject.Dal.Mongo.Entities;

/// <summary>
/// Mongo representation of the ProductModel.
/// </summary>
public class ProductEntity
{
    [BsonId] public int Id { get; set; }
    public required string Name { get; set; } = null!;
    public required float Price { get; set; }
    public required uint StockCount { get; set; }
    public string? Description { get; set; }
    public string[] Categories { get; set; } = Array.Empty<string>();
}