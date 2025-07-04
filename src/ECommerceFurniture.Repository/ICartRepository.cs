using System.Threading.Tasks;
using ECommerceFurniture.Domain;

namespace ECommerceFurniture.Repository
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartBySessionIdAsync(string sessionId);
        Task<Cart?> GetCartWithItemsAsync(string sessionId);
    }
} 