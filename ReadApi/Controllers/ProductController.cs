using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PDBProject.Dal.Mongo.Entities;
using PDBProject.Dal.Mongo.Services;

namespace PDBProject.ReadApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{id:int}")]
    public async Task<Results<NotFound, Ok<ProductEntity>>> GetProductById(int id)
    {
        var existingProduct = await _productService.GetAsyncById(id);
        if (existingProduct is null) return TypedResults.NotFound();

        return TypedResults.Ok(existingProduct);
    }

    [HttpGet]
    public async Task<Ok<List<ProductEntity>>> GetAllProducts()
    {
        var allProducts = await _productService.GetAsyncAll();

        return TypedResults.Ok(allProducts);
    }

    [HttpGet("{price:int}/{upperBorderPrice:int}")]
    public async Task<Ok<List<ProductEntity>>> GetProductsByPrice(int price, int upperBorderPrice)
    {
        var allProductEqualToPrice = await _productService.GetAsyncByPrice(price, upperBorderPrice);

        return TypedResults.Ok(allProductEqualToPrice);
    }

    [HttpGet("{category}")]
    public async Task<Ok<List<ProductEntity>>> GetProductsByCategory(string category)
    {
        var allProductEqualToCategory = await _productService.GetAsyncByCategory(category);

        return TypedResults.Ok(allProductEqualToCategory);
    }
}