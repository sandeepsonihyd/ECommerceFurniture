using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceFurniture.Domain;

namespace ECommerceFurniture.Repository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesWithProductCountAsync();
    }
} 