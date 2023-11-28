using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ReadApi.Configurations;
using ReadApi.Models;

namespace ReadApi.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<ProductModel> _productCollection;

        public ProductService(IOptions<DatabaseSettings> databaseSettings)
        {
            MongoClient mongoClient = new MongoClient(databaseSettings.Value.ConnectionStrings);
            IMongoDatabase mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _productCollection = mongoDb.GetCollection<ProductModel>(databaseSettings.Value.ProductCollection);
        }

        public async Task<List<ProductModel>> GetAsyncAll() => await _productCollection.Find(_ => true).ToListAsync();
        public async Task<ProductModel> GetAsyncById(string id) => await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<ProductModel>> GetAsyncByPrice(int lower_border_price, int upper_border_price) => await _productCollection.Find(x => x.price >= lower_border_price && x.price <= upper_border_price).ToListAsync();
        
        public async Task<List<ProductModel>> GetAsyncByCategory(string category) => await _productCollection.Find(x => x.categoryName == category).ToListAsync();
        public async Task CreateAsync(ProductModel User) => await _productCollection.InsertOneAsync(User);

        public async Task UpdateAsync(ProductModel User) => await _productCollection.ReplaceOneAsync(x => x.Id == User.Id, User);

        public async Task RemoveAsync(string id) => await _productCollection.DeleteOneAsync(x => x.Id == id);

    }
}
