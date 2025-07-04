using System.Threading.Tasks;
using ECommerceFurniture.Business.DTOs;

namespace ECommerceFurniture.Business.Services
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(string sessionId);
        Task<CartItemDto> AddToCartAsync(AddToCartDto addToCartDto);
        Task<CartItemDto?> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto);
        Task<bool> RemoveFromCartAsync(int cartItemId);
        Task ClearCartAsync(string sessionId);
    }
} 