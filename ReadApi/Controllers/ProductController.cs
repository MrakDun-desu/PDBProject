using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ReadApi.Models;
using ReadApi.Services;

namespace ReadApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService) => _productService = productService;

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var existingProduct = await _productService.GetAsyncById(id);
            if (existingProduct is null)
            {
                return NotFound();
            }

            return Ok(existingProduct);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var allProducts = await _productService.GetAsyncAll();

            if (allProducts.Any())
            {
                return Ok(allProducts);
            }
            return NotFound();
        }

        [HttpGet("{price}/{upper_border_price}")]
        public async Task<IActionResult> GetProductsByPrice(int price, int upper_border_price)
        {
            var allProductEqualToPrice = await _productService.GetAsyncByPrice(price, upper_border_price);

            if (allProductEqualToPrice.Any())
            {
                return Ok(allProductEqualToPrice);
            }
            return NotFound();
        }

        [HttpGet("{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var allProductEqualToCategory = await _productService.GetAsyncByCategory(category);

            if (allProductEqualToCategory.Any())
            {
                return Ok(allProductEqualToCategory);
            }
            return NotFound();
        }
    }
}
