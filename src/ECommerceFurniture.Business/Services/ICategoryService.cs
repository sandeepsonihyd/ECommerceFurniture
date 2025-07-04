using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceFurniture.Business.DTOs;

namespace ECommerceFurniture.Business.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync();
    }
} 