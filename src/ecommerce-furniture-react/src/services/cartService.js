import api from './api';

export const cartService = {
  // Get cart by session ID
  getCart: async (sessionId) => {
    try {
      const response = await api.get(`/cart/${sessionId}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching cart for session ${sessionId}:`, error);
      throw error;
    }
  },

  // Add item to cart
  addToCart: async (addToCartData) => {
    try {
      const response = await api.post('/cart/add', addToCartData);
      return response.data;
    } catch (error) {
      console.error('Error adding item to cart:', error);
      throw error;
    }
  },

  // Update cart item quantity
  updateCartItem: async (updateCartItemData) => {
    try {
      const response = await api.put('/cart/update', updateCartItemData);
      return response.data;
    } catch (error) {
      console.error('Error updating cart item:', error);
      throw error;
    }
  },

  // Remove item from cart
  removeFromCart: async (cartItemId) => {
    try {
      const response = await api.delete(`/cart/remove/${cartItemId}`);
      return response.data;
    } catch (error) {
      console.error(`Error removing cart item ${cartItemId}:`, error);
      throw error;
    }
  },

  // Clear entire cart
  clearCart: async (sessionId) => {
    try {
      const response = await api.delete(`/cart/clear/${sessionId}`);
      return response.data;
    } catch (error) {
      console.error(`Error clearing cart for session ${sessionId}:`, error);
      throw error;
    }
  }
}; 