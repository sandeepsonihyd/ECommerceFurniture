using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ECommerceFurniture.DataAccess;
using ECommerceFurniture.Domain;

namespace ECommerceFurniture.Repository
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(ECommerceFurnitureDbContext context) : base(context)
        {
        }

        public async Task<Cart?> GetCartBySessionIdAsync(string sessionId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        }

        public async Task<Cart?> GetCartWithItemsAsync(string sessionId)
        {
            return await _dbSet
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        }
    }
} 