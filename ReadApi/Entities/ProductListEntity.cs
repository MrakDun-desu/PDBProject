using MongoDB.Bson.Serialization.Attributes;

namespace PDBProject.ReadApi.Models;

public class ProductListEntity
{
    [BsonId] public int Id { get; init; }

    public required int ProductCount { get; set; }
}