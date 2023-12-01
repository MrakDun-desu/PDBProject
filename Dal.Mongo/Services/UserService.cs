using MongoDB.Driver;
using PDBProject.Dal.Mongo.Configurations;
using PDBProject.Dal.Mongo.Entities;

namespace PDBProject.Dal.Mongo.Services;

public class UserService
{
    private readonly IMongoCollection<UserEntity> _userCollection;

    public UserService(DatabaseSettings databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.ConnectionStrings);
        var mongoDb = mongoClient.GetDatabase(databaseSettings.DatabaseName);

        if (!mongoDb.ListCollectionNames().ToList().Contains(databaseSettings.UserCollection))
            // if collection doesn't exist at startup time, we create it just to be sure
            mongoDb.CreateCollection(databaseSettings.UserCollection);
        _userCollection = mongoDb.GetCollection<UserEntity>(databaseSettings.UserCollection);
    }

    public async Task<List<UserEntity>> GetAsync()
    {
        return await _userCollection.Find(_ => true).ToListAsync();
    }

    public async Task<UserEntity?> GetAsync(int id)
    {
        return await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(UserEntity user)
    {
        await _userCollection.InsertOneAsync(user);
    }

    public async Task UpdateAsync(UserEntity user)
    {
        await _userCollection.ReplaceOneAsync(x => x.Id == user.Id, user);
    }

    public async Task RemoveAsync(int id)
    {
        await _userCollection.DeleteOneAsync(x => x.Id == id);
    }
}