using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerceFurniture.DataAccess;
using ECommerceFurniture.Domain;

namespace ECommerceFurniture.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ECommerceFurnitureDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.Specifications)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var lowerSearchTerm = searchTerm.ToLower();

            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive && 
                           (p.Name.ToLower().Contains(lowerSearchTerm) ||
                            p.Description.ToLower().Contains(lowerSearchTerm) ||
                            p.SKU.ToLower().Contains(lowerSearchTerm)))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
        {
            // For this demo, we'll consider products with stock > 10 as featured
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive && p.StockQuantity > 10)
                .OrderByDescending(p => p.StockQuantity)
                .Take(6)
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.ProductImages.OrderBy(img => img.DisplayOrder))
                .Include(p => p.Specifications.OrderBy(spec => spec.DisplayOrder))
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
} 