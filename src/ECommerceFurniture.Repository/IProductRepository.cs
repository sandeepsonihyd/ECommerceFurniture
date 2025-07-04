using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceFurniture.Domain;

namespace ECommerceFurniture.Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<Product>> GetFeaturedProductsAsync();
        Task<Product?> GetProductWithDetailsAsync(int id);
    }
} 