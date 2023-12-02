using MongoDB.Bson.Serialization.Attributes;

namespace PDBProject.Dal.Mongo.Entities;

/// <summary>
/// Mongo representation of the OrderItemModel.
/// </summary>
public class OrderItemEntity
{
    [BsonId] public int ProductId { get; init; }

    public required uint ProductCount { get; set; }
}