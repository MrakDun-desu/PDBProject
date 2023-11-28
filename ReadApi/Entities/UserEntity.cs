using MongoDB.Bson.Serialization.Attributes;

namespace PDBProject.ReadApi.Models;

public class UserEntity
{
    [BsonId] public int Id { get; set; }

    public required string Name { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public List<OrderEntity>? Orders { get; set; }
}