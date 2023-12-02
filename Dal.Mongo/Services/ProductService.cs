using MongoDB.Driver;
using PDBProject.Dal.Mongo.Configurations;
using PDBProject.Dal.Mongo.Entities;

namespace PDBProject.Dal.Mongo.Services;

/// <summary>
/// Service that can be used to execute commands and queries on the product collection in the Mongo database.
/// </summary>
public class ProductService
{
    private readonly IMongoCollection<ProductEntity> _productCollection;

    /// <param name="databaseSettings">Settings of the Mongo database that we should connect to.</param>
    public ProductService(DatabaseSettings databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.ConnectionStrings);
        var mongoDb = mongoClient.GetDatabase(databaseSettings.DatabaseName);

        // if collection doesn't exist at startup time, we create it just to be sure
        if (!mongoDb.ListCollectionNames().ToList().Contains(databaseSettings.ProductCollection))
        {
            mongoDb.CreateCollection(databaseSettings.ProductCollection);
        }

        _productCollection = mongoDb.GetCollection<ProductEntity>(databaseSettings.ProductCollection);
    }

    /// <summary>
    /// Asynchronously retrieves all the products from the database.
    /// </summary>
    /// <returns>List of all products without any filters.</returns>
    public async Task<List<ProductEntity>> GetAsyncAll()
    {
        var findResults = await _productCollection.FindAsync(_ => true);
        return await findResults.ToListAsync();
    }

    /// <summary>
    /// Asynchronously retrieves a single product.
    /// </summary>
    /// <param name="id">Id of the product that should be retrieved.</param>
    /// <returns>Product if it has been found, or null if it hasn't been found.</returns>
    public async Task<ProductEntity?> GetAsyncById(int id)
    {
        return await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Asynchronously retrieves a list of products in a certain price range.
    /// </summary>
    /// <param name="lowerBorderPrice">Only products with price greater or equal to this value will be matched.</param>
    /// <param name="upperBorderPrice">Only products with price lower or equal to this value will be matched.</param>
    /// <returns>List of products in the provided price range.</returns>
    public async Task<List<ProductEntity>> GetAsyncByPrice(float lowerBorderPrice, float upperBorderPrice)
    {
        return await _productCollection.Find(x => x.Price >= lowerBorderPrice && x.Price <= upperBorderPrice)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously retrieves a list of products in a certain category.
    /// </summary>
    /// <param name="category">Only products that belong in this category will be matched. Case-insensitive.</param>
    /// <returns>List of products that belong in the given category.</returns>
    public async Task<List<ProductEntity>> GetAsyncByCategory(string category)
    {
        return await _productCollection.Find(x => x.Categories.Contains(category.ToLowerInvariant())).ToListAsync();
    }

    /// <summary>
    /// Asynchronously inserts a product into the database.
    /// </summary>
    /// <param name="product">Product to be inserted.</param>
    public async Task CreateAsync(ProductEntity product)
    {
        await _productCollection.InsertOneAsync(product);
    }

    /// <summary>
    /// Asynchronously updates a product.
    /// </summary>
    /// <param name="product">Product to be updated.</param>
    public async Task UpdateAsync(ProductEntity product)
    {
        await _productCollection.ReplaceOneAsync(x => x.Id == product.Id, product);
    }

    /// <summary>
    /// Asynchronously removed a product from the database.
    /// </summary>
    /// <param name="id">Id of the product to be removed.</param>
    public async Task RemoveAsync(int id)
    {
        await _productCollection.DeleteOneAsync(x => x.Id == id);
    }
}