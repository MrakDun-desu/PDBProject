namespace PDBProject.Dal.Mongo.Configurations;

public class DatabaseSettings
{
    public required string ConnectionStrings { get; set; } = null!;
    public required string DatabaseName { get; set; } = null!;
    public required string UserCollection { get; set; } = null!;
    public required string ProductCollection { get; set; } = null!;
}