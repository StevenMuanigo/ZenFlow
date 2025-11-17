using ZenFlow.CatalogService.DTOs;
using ZenFlow.CatalogService.Models;

namespace ZenFlow.CatalogService.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<ProductDto> GetProductByIdAsync(string id);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
        Task<ProductDto> CreateProductAsync(CreateProductDto productDto);
        Task<ProductDto> UpdateProductAsync(string id, UpdateProductDto productDto);
        Task<bool> DeleteProductAsync(string id);
    }
}