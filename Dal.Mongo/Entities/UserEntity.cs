using MongoDB.Bson.Serialization.Attributes;

namespace PDBProject.Dal.Mongo.Entities;

public class UserEntity
{
    [BsonId] public int Id { get; init; }

    public required string Name { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public List<OrderEntity>? Orders { get; set; }
}