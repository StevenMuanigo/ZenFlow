using MongoDB.Driver;
using ZenFlow.CatalogService.Models;

namespace ZenFlow.CatalogService.Data
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}