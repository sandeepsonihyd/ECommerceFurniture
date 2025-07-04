using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceFurniture.Business.DTOs;
using ECommerceFurniture.Domain;
using ECommerceFurniture.Repository;

namespace ECommerceFurniture.Business.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CartDto> GetCartAsync(string sessionId)
        {
            var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(sessionId);
            
            if (cart == null)
            {
                // Create a new cart if it doesn't exist
                cart = new Cart
                {
                    SessionId = sessionId,
                    CreatedDate = DateTime.UtcNow
                };
                
                await _unitOfWork.Carts.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }

            return MapToCartDto(cart);
        }

        public async Task<CartItemDto> AddToCartAsync(AddToCartDto addToCartDto)
        {
            // Get or create cart
            var cart = await _unitOfWork.Carts.GetCartBySessionIdAsync(addToCartDto.SessionId);
            if (cart == null)
            {
                cart = new Cart
                {
                    SessionId = addToCartDto.SessionId,
                    CreatedDate = DateTime.UtcNow
                };
                await _unitOfWork.Carts.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }

            // Get product
            var product = await _unitOfWork.Products.GetByIdAsync(addToCartDto.ProductId);
            if (product == null)
            {
                throw new ArgumentException("Product not found", nameof(addToCartDto.ProductId));
            }

            // Check if item already exists in cart
            var existingCartItem = await _unitOfWork.CartItems.GetCartItemByCartAndProductAsync(cart.Id, addToCartDto.ProductId);
            
            if (existingCartItem != null)
            {
                // Update quantity
                existingCartItem.Quantity += addToCartDto.Quantity;
                existingCartItem.ModifiedDate = DateTime.UtcNow;
                _unitOfWork.CartItems.Update(existingCartItem);
            }
            else
            {
                // Add new cart item
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = addToCartDto.ProductId,
                    Quantity = addToCartDto.Quantity,
                    UnitPrice = product.Price,
                    CreatedDate = DateTime.UtcNow
                };
                
                await _unitOfWork.CartItems.AddAsync(cartItem);
                existingCartItem = cartItem;
            }

            await _unitOfWork.SaveChangesAsync();

            // Return the cart item with product details
            var updatedCartItem = await _unitOfWork.CartItems.GetCartItemByCartAndProductAsync(cart.Id, addToCartDto.ProductId);
            return MapToCartItemDto(updatedCartItem!);
        }

        public async Task<CartItemDto?> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(updateCartItemDto.CartItemId);
            if (cartItem == null)
            {
                return null;
            }

            if (updateCartItemDto.Quantity <= 0)
            {
                _unitOfWork.CartItems.Remove(cartItem);
                await _unitOfWork.SaveChangesAsync();
                return null;
            }

            cartItem.Quantity = updateCartItemDto.Quantity;
            cartItem.ModifiedDate = DateTime.UtcNow;
            _unitOfWork.CartItems.Update(cartItem);
            await _unitOfWork.SaveChangesAsync();

            var updatedCartItem = await _unitOfWork.CartItems.GetByIdAsync(updateCartItemDto.CartItemId);
            return MapToCartItemDto(updatedCartItem!);
        }

        public async Task<bool> RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(cartItemId);
            if (cartItem == null)
            {
                return false;
            }

            _unitOfWork.CartItems.Remove(cartItem);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task ClearCartAsync(string sessionId)
        {
            var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(sessionId);
            if (cart != null && cart.CartItems.Any())
            {
                _unitOfWork.CartItems.RemoveRange(cart.CartItems);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        private static CartDto MapToCartDto(Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                SessionId = cart.SessionId,
                Items = cart.CartItems?.Select(MapToCartItemDto).ToList() ?? new List<CartItemDto>(),
                TotalAmount = cart.TotalAmount,
                TotalItems = cart.TotalItems
            };
        }

        private static CartItemDto MapToCartItemDto(CartItem cartItem)
        {
            var primaryImage = cartItem.Product?.ProductImages?.FirstOrDefault(img => img.IsPrimary)?.ImageUrl ??
                              cartItem.Product?.ProductImages?.FirstOrDefault()?.ImageUrl ?? "";

            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product?.Name ?? "",
                ProductSKU = cartItem.Product?.SKU ?? "",
                ProductImageUrl = primaryImage,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
                TotalPrice = cartItem.TotalPrice
            };
        }
    }
} 