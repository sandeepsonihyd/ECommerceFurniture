using Microsoft.AspNetCore.Mvc;
using ECommerceFurniture.Business.Services;
using ECommerceFurniture.Business.DTOs;

namespace ECommerceFurniture.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{sessionId}")]
        public async Task<ActionResult<CartDto>> GetCart(string sessionId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sessionId))
                {
                    return BadRequest("Session ID cannot be empty.");
                }

                var cart = await _cartService.GetCartAsync(sessionId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult<CartItemDto>> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            try
            {
                if (addToCartDto == null)
                {
                    return BadRequest("Invalid request data.");
                }

                if (string.IsNullOrWhiteSpace(addToCartDto.SessionId))
                {
                    return BadRequest("Session ID is required.");
                }

                if (addToCartDto.ProductId <= 0)
                {
                    return BadRequest("Valid Product ID is required.");
                }

                if (addToCartDto.Quantity <= 0)
                {
                    return BadRequest("Quantity must be greater than 0.");
                }

                var cartItem = await _cartService.AddToCartAsync(addToCartDto);
                return Ok(cartItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<CartItemDto>> UpdateCartItem([FromBody] UpdateCartItemDto updateCartItemDto)
        {
            try
            {
                if (updateCartItemDto == null)
                {
                    return BadRequest("Invalid request data.");
                }

                if (updateCartItemDto.CartItemId <= 0)
                {
                    return BadRequest("Valid Cart Item ID is required.");
                }

                if (updateCartItemDto.Quantity < 0)
                {
                    return BadRequest("Quantity cannot be negative.");
                }

                var cartItem = await _cartService.UpdateCartItemAsync(updateCartItemDto);
                
                if (cartItem == null)
                {
                    return NotFound("Cart item not found or was removed.");
                }

                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("remove/{cartItemId}")]
        public async Task<ActionResult> RemoveFromCart(int cartItemId)
        {
            try
            {
                if (cartItemId <= 0)
                {
                    return BadRequest("Valid Cart Item ID is required.");
                }

                var result = await _cartService.RemoveFromCartAsync(cartItemId);
                
                if (!result)
                {
                    return NotFound("Cart item not found.");
                }

                return Ok(new { message = "Item removed from cart successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("clear/{sessionId}")]
        public async Task<ActionResult> ClearCart(string sessionId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sessionId))
                {
                    return BadRequest("Session ID cannot be empty.");
                }

                await _cartService.ClearCartAsync(sessionId);
                return Ok(new { message = "Cart cleared successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
} 