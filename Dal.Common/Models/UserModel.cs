namespace PDBProject.Dal.Common.Models;

/// <summary>
/// Represents a user of the e-shop.
/// </summary>
public record UserModel
{
    public required int Id { get; init; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}