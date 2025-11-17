using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenFlow.CatalogService.DTOs;
using ZenFlow.CatalogService.Services;

namespace ZenFlow.CatalogService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { status = "Catalog Service is running", timestamp = DateTime.UtcNow });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(string category)
        {
            var products = await _productService.GetProductsByCategoryAsync(category);
            return Ok(products);
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts(string searchTerm)
        {
            var products = await _productService.SearchProductsAsync(searchTerm);
            return Ok(products);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto productDto)
        {
            var product = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductDto>> UpdateProduct(string id, UpdateProductDto productDto)
        {
            var product = await _productService.UpdateProductAsync(id, productDto);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteProduct(string id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}