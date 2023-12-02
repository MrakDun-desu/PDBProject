namespace PDBProject.WriteApi.Entities;

/// <summary>
/// Postgres representation of the UserModel.
/// </summary>
public record UserEntity
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
}