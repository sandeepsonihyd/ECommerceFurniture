using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceFurniture.Business.DTOs;

namespace ECommerceFurniture.Business.Services
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
        Task<PagedResult<ProductDto>> GetProductsPagedAsync(ProductFilterDto filter);
        Task<IEnumerable<ProductDto>> GetFeaturedProductsAsync();
    }
} 