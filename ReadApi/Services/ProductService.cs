using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PDBProject.ReadApi.Configurations;
using PDBProject.ReadApi.Models;

namespace PDBProject.ReadApi.Services;

public class ProductService
{
    private readonly IMongoCollection<ProductEntity> _productCollection;

    public ProductService(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionStrings);
        var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        if (!mongoDb.ListCollectionNames().ToList().Contains(databaseSettings.Value.ProductCollection))
            // if collection doesn't exist at startup time, we create it just to be sure
            mongoDb.CreateCollection(databaseSettings.Value.ProductCollection);
        _productCollection = mongoDb.GetCollection<ProductEntity>(databaseSettings.Value.ProductCollection);
    }

    public async Task<List<ProductEntity>> GetAsyncAll()
    {
        var findResults = await _productCollection.FindAsync(_ => true);
        return await findResults.ToListAsync();
    }

    public async Task<ProductEntity?> GetAsyncById(int id)
    {
        return await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<ProductEntity>> GetAsyncByPrice(int lowerBorderPrice, int upperBorderPrice)
    {
        return await _productCollection.Find(x => x.Price >= lowerBorderPrice && x.Price <= upperBorderPrice)
            .ToListAsync();
    }

    public async Task<List<ProductEntity>> GetAsyncByCategory(string category)
    {
        return await _productCollection.Find(x => x.Categories.Contains(category)).ToListAsync();
    }

    public async Task CreateAsync(ProductEntity user)
    {
        await _productCollection.InsertOneAsync(user);
    }

    public async Task UpdateAsync(ProductEntity user)
    {
        await _productCollection.ReplaceOneAsync(x => x.Id == user.Id, user);
    }

    public async Task RemoveAsync(int id)
    {
        await _productCollection.DeleteOneAsync(x => x.Id == id);
    }
}