using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ReadApi.Configurations;
using ReadApi.Models;

namespace ReadApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<UserModel> _userCollection;

        public UserService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionStrings);
            var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _userCollection = mongoDb.GetCollection<UserModel>(databaseSettings.Value.UserCollection);
        }

        public async Task<List<UserModel>> GetAsync() => await _userCollection.Find(_ =>  true).ToListAsync();
        public async Task<UserModel> GetAsync(string id) => await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(UserModel User) => await _userCollection.InsertOneAsync(User);

        public async Task UpdateAsync(UserModel User) => await _userCollection.ReplaceOneAsync(x => x.Id == User.Id, User);

        public async Task RemoveAsync(string id) => await _userCollection.DeleteOneAsync(x => x.Id == id);

    }
}
