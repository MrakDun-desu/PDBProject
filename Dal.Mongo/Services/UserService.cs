using MongoDB.Driver;
using PDBProject.Dal.Mongo.Configurations;
using PDBProject.Dal.Mongo.Entities;

namespace PDBProject.Dal.Mongo.Services;

/// <summary>
/// Service that can be used to execute commands and queries on the product collection in the Mongo database.
/// </summary>
public class UserService
{
    private readonly IMongoCollection<UserEntity> _userCollection;

    /// <param name="databaseSettings">Settings of the Mongo database that we should connect to.</param>
    public UserService(DatabaseSettings databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.ConnectionStrings);
        var mongoDb = mongoClient.GetDatabase(databaseSettings.DatabaseName);

        if (!mongoDb.ListCollectionNames().ToList().Contains(databaseSettings.UserCollection))
            // if collection doesn't exist at startup time, we create it just to be sure
            mongoDb.CreateCollection(databaseSettings.UserCollection);
        _userCollection = mongoDb.GetCollection<UserEntity>(databaseSettings.UserCollection);
    }

    /// <summary>
    /// Asynchronously retrieves all users from the database.
    /// </summary>
    /// <returns>List of all users.</returns>
    public async Task<List<UserEntity>> GetAsync()
    {
        return await _userCollection.Find(_ => true).ToListAsync();
    }

    /// <summary>
    /// Asynchronously retrieves a single user.
    /// </summary>
    /// <param name="id">Id of the user that should be retrieved.</param>
    /// <returns>User, if it has been found, or null if it hasn't been found.</returns>
    public async Task<UserEntity?> GetAsync(int id)
    {
        return await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Inserts a new user into the database.
    /// </summary>
    /// <param name="user">User to be inserted.</param>
    public async Task CreateAsync(UserEntity user)
    {
        await _userCollection.InsertOneAsync(user);
    }

    /// <summary>
    /// Updates a user within the database.
    /// </summary>
    /// <param name="user">User to be updated.</param>
    public async Task UpdateAsync(UserEntity user)
    {
        await _userCollection.ReplaceOneAsync(x => x.Id == user.Id, user);
    }

    /// <summary>
    /// Removes a user from the database.
    /// </summary>
    /// <param name="id">Id of the user to be removed.</param>
    public async Task RemoveAsync(int id)
    {
        await _userCollection.DeleteOneAsync(x => x.Id == id);
    }
}