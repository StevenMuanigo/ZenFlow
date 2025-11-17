using MongoDB.Driver;
using MongoDB.Bson;
using ZenFlow.CatalogService.Data;
using ZenFlow.CatalogService.DTOs;
using ZenFlow.CatalogService.Models;

namespace ZenFlow.CatalogService.Services
{
    public class ProductService : IProductService
    {
        private readonly ICatalogContext _context;

        public ProductService(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _context.Products.Find(p => true).ToListAsync();
            return products.Select(p => MapToDto(p));
        }

        public async Task<ProductDto> GetProductByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid product ID format");
            }

            var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
            return product == null ? null : MapToDto(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category)
        {
            var products = await _context.Products.Find(p => p.Category == category).ToListAsync();
            return products.Select(p => MapToDto(p));
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            var filter = Builders<Product>.Filter.Or(
                Builders<Product>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                Builders<Product>.Filter.Regex(p => p.Description, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                Builders<Product>.Filter.Regex(p => p.Category, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
            );

            var products = await _context.Products.Find(filter).ToListAsync();
            return products.Select(p => MapToDto(p));
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Category = productDto.Category,
                Calories = productDto.Calories,
                Protein = productDto.Protein,
                Carbohydrates = productDto.Carbohydrates,
                Fat = productDto.Fat,
                ServingSize = productDto.ServingSize,
                Allergens = productDto.Allergens,
                DietaryTags = productDto.DietaryTags
            };

            await _context.Products.InsertOneAsync(product);
            return MapToDto(product);
        }

        public async Task<ProductDto> UpdateProductAsync(string id, UpdateProductDto productDto)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid product ID format");
            }

            var update = Builders<Product>.Update
                .Set(p => p.Name, productDto.Name)
                .Set(p => p.Description, productDto.Description)
                .Set(p => p.Category, productDto.Category)
                .Set(p => p.Calories, productDto.Calories)
                .Set(p => p.Protein, productDto.Protein)
                .Set(p => p.Carbohydrates, productDto.Carbohydrates)
                .Set(p => p.Fat, productDto.Fat)
                .Set(p => p.ServingSize, productDto.ServingSize)
                .Set(p => p.Allergens, productDto.Allergens)
                .Set(p => p.DietaryTags, productDto.DietaryTags)
                .Set(p => p.UpdatedAt, DateTime.UtcNow);

            var result = await _context.Products.UpdateOneAsync(p => p.Id == id, update);
            if (result.ModifiedCount == 0)
            {
                return null;
            }

            var updatedProduct = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
            return MapToDto(updatedProduct);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid product ID format");
            }

            var result = await _context.Products.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }

        private ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Calories = product.Calories,
                Protein = product.Protein,
                Carbohydrates = product.Carbohydrates,
                Fat = product.Fat,
                ServingSize = product.ServingSize,
                Allergens = product.Allergens,
                DietaryTags = product.DietaryTags
            };
        }
    }
}