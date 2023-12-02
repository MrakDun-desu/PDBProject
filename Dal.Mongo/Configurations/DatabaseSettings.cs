namespace PDBProject.Dal.Mongo.Configurations;

public class DatabaseSettings
{
    /// <summary>
    /// Connection string to the Mongo database.
    /// </summary>
    public required string ConnectionStrings { get; set; } = null!;
    /// <summary>
    /// Name of the database in the given connection.
    /// </summary>
    public required string DatabaseName { get; set; } = null!;
    /// <summary>
    /// Name of the user collection.
    /// </summary>
    public required string UserCollection { get; set; } = null!;
    /// <summary>
    /// Name of the product collection.
    /// </summary>
    public required string ProductCollection { get; set; } = null!;
}