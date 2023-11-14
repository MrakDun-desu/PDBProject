namespace WriteApi.Models;

public record UserRegisterModel
{
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}
