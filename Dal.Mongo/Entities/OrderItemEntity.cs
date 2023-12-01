using MongoDB.Bson.Serialization.Attributes;

namespace PDBProject.Dal.Mongo.Entities;

public class OrderItemEntity
{
    [BsonId] public int ProductId { get; init; }

    public required uint ProductCount { get; set; }
}