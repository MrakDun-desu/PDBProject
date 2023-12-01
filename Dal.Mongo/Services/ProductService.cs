using MongoDB.Driver;
using PDBProject.Dal.Mongo.Configurations;
using PDBProject.Dal.Mongo.Entities;

namespace PDBProject.Dal.Mongo.Services;

public class ProductService
{
    private readonly IMongoCollection<ProductEntity> _productCollection;

    public ProductService(DatabaseSettings databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.ConnectionStrings);
        var mongoDb = mongoClient.GetDatabase(databaseSettings.DatabaseName);

        if (!mongoDb.ListCollectionNames().ToList().Contains(databaseSettings.ProductCollection))
            // if collection doesn't exist at startup time, we create it just to be sure
            mongoDb.CreateCollection(databaseSettings.ProductCollection);
        _productCollection = mongoDb.GetCollection<ProductEntity>(databaseSettings.ProductCollection);
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

    public async Task CreateAsync(ProductEntity product)
    {
        await _productCollection.InsertOneAsync(product);
    }

    public async Task UpdateAsync(ProductEntity product)
    {
        await _productCollection.ReplaceOneAsync(x => x.Id == product.Id, product);
    }

    public async Task RemoveAsync(int id)
    {
        await _productCollection.DeleteOneAsync(x => x.Id == id);
    }
}