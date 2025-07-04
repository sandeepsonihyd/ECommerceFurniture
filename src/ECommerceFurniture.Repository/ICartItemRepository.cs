using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceFurniture.Domain;

namespace ECommerceFurniture.Repository
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);
        Task<CartItem?> GetCartItemByCartAndProductAsync(int cartId, int productId);
    }
} 