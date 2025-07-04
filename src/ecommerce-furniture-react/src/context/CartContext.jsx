import React, { createContext, useContext, useReducer, useEffect } from 'react';
import { cartService } from '../services/api';
import toast from 'react-hot-toast';

// Create the context
const CartContext = createContext();

// Cart actions
const CART_ACTIONS = {
  SET_CART: 'SET_CART',
  ADD_ITEM: 'ADD_ITEM',
  UPDATE_ITEM: 'UPDATE_ITEM',
  REMOVE_ITEM: 'REMOVE_ITEM',
  CLEAR_CART: 'CLEAR_CART',
  SET_LOADING: 'SET_LOADING'
};

// Initial state
const initialState = {
  cart: {
    id: null,
    sessionId: null,
    items: [],
    totalAmount: 0,
    totalItems: 0
  },
  loading: false
};

// Cart reducer
const cartReducer = (state, action) => {
  switch (action.type) {
    case CART_ACTIONS.SET_CART:
      return {
        ...state,
        cart: action.payload,
        loading: false
      };
    
    case CART_ACTIONS.ADD_ITEM:
      const newItem = action.payload;
      const existingItem = state.cart.items.find(item => item.productId === newItem.productId);
      
      let updatedItems;
      if (existingItem) {
        updatedItems = state.cart.items.map(item =>
          item.productId === newItem.productId
            ? { ...item, quantity: item.quantity + newItem.quantity, totalPrice: (item.quantity + newItem.quantity) * item.unitPrice }
            : item
        );
      } else {
        updatedItems = [...state.cart.items, newItem];
      }
      
      const newTotalItems = updatedItems.reduce((total, item) => total + item.quantity, 0);
      const newTotalAmount = updatedItems.reduce((total, item) => total + item.totalPrice, 0);
      
      return {
        ...state,
        cart: {
          ...state.cart,
          items: updatedItems,
          totalItems: newTotalItems,
          totalAmount: newTotalAmount
        }
      };
    
    case CART_ACTIONS.UPDATE_ITEM:
      const { itemId, quantity } = action.payload;
      const updatedItemsList = state.cart.items.map(item =>
        item.id === itemId
          ? { ...item, quantity, totalPrice: quantity * item.unitPrice }
          : item
      );
      
      const updatedTotalItems = updatedItemsList.reduce((total, item) => total + item.quantity, 0);
      const updatedTotalAmount = updatedItemsList.reduce((total, item) => total + item.totalPrice, 0);
      
      return {
        ...state,
        cart: {
          ...state.cart,
          items: updatedItemsList,
          totalItems: updatedTotalItems,
          totalAmount: updatedTotalAmount
        }
      };
    
    case CART_ACTIONS.REMOVE_ITEM:
      const filteredItems = state.cart.items.filter(item => item.id !== action.payload);
      const filteredTotalItems = filteredItems.reduce((total, item) => total + item.quantity, 0);
      const filteredTotalAmount = filteredItems.reduce((total, item) => total + item.totalPrice, 0);
      
      return {
        ...state,
        cart: {
          ...state.cart,
          items: filteredItems,
          totalItems: filteredTotalItems,
          totalAmount: filteredTotalAmount
        }
      };
    
    case CART_ACTIONS.CLEAR_CART:
      return {
        ...state,
        cart: {
          ...state.cart,
          items: [],
          totalItems: 0,
          totalAmount: 0
        }
      };
    
    case CART_ACTIONS.SET_LOADING:
      return {
        ...state,
        loading: action.payload
      };
    
    default:
      return state;
  }
};

// Generate session ID
const generateSessionId = () => {
  return `session_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
};

// Get session ID from localStorage or generate new one
const getSessionId = () => {
  let sessionId = localStorage.getItem('cart_session_id');
  if (!sessionId) {
    sessionId = generateSessionId();
    localStorage.setItem('cart_session_id', sessionId);
  }
  return sessionId;
};

// Cart provider component
export const CartProvider = ({ children }) => {
  const [state, dispatch] = useReducer(cartReducer, initialState);
  
  // Initialize cart on component mount
  useEffect(() => {
    const initializeCart = async () => {
      try {
        dispatch({ type: CART_ACTIONS.SET_LOADING, payload: true });
        const sessionId = getSessionId();
        const response = await cartService.getCart(sessionId);
        dispatch({ type: CART_ACTIONS.SET_CART, payload: response.data });
      } catch (error) {
        console.error('Error initializing cart:', error);
        // If cart doesn't exist, start with empty cart
        dispatch({ 
          type: CART_ACTIONS.SET_CART, 
          payload: {
            id: null,
            sessionId: getSessionId(),
            items: [],
            totalAmount: 0,
            totalItems: 0
          }
        });
      }
    };

    initializeCart();
  }, []);

  // Add item to cart
  const addToCart = async (product, quantity = 1) => {
    try {
      const sessionId = getSessionId();
      const response = await cartService.addToCart({
        sessionId,
        productId: product.id,
        quantity
      });
      
      // Update local state with the new item
      const newItem = {
        id: response.data.id,
        productId: product.id,
        productName: product.name,
        productSKU: product.sku,
        productImageUrl: product.images?.find(img => img.isPrimary)?.imageUrl || '',
        quantity,
        unitPrice: product.price,
        totalPrice: quantity * product.price
      };
      
      dispatch({ type: CART_ACTIONS.ADD_ITEM, payload: newItem });
      
      // Refresh cart from server to ensure consistency
      await refreshCart();
    } catch (error) {
      console.error('Error adding to cart:', error);
      toast.error('Failed to add item to cart');
    }
  };

  // Update cart item quantity
  const updateCartItem = async (cartItemId, quantity) => {
    try {
      await cartService.updateCartItem({ cartItemId, quantity });
      dispatch({ type: CART_ACTIONS.UPDATE_ITEM, payload: { itemId: cartItemId, quantity } });
    } catch (error) {
      console.error('Error updating cart item:', error);
      toast.error('Failed to update item');
      // Refresh cart to ensure consistency
      await refreshCart();
    }
  };

  // Remove item from cart
  const removeFromCart = async (cartItemId) => {
    try {
      await cartService.removeFromCart(cartItemId);
      dispatch({ type: CART_ACTIONS.REMOVE_ITEM, payload: cartItemId });
    } catch (error) {
      console.error('Error removing from cart:', error);
      toast.error('Failed to remove item');
      // Refresh cart to ensure consistency
      await refreshCart();
    }
  };

  // Clear entire cart
  const clearCart = async () => {
    try {
      const sessionId = getSessionId();
      await cartService.clearCart(sessionId);
      dispatch({ type: CART_ACTIONS.CLEAR_CART });
    } catch (error) {
      console.error('Error clearing cart:', error);
      toast.error('Failed to clear cart');
    }
  };

  // Refresh cart from server
  const refreshCart = async () => {
    try {
      const sessionId = getSessionId();
      const response = await cartService.getCart(sessionId);
      dispatch({ type: CART_ACTIONS.SET_CART, payload: response.data });
    } catch (error) {
      console.error('Error refreshing cart:', error);
    }
  };

  // Get total items in cart
  const getTotalItems = () => {
    return state.cart.totalItems || 0;
  };

  // Get total amount in cart
  const getTotalAmount = () => {
    return state.cart.totalAmount || 0;
  };

  // Check if item is in cart
  const isInCart = (productId) => {
    return state.cart.items.some(item => item.productId === productId);
  };

  const value = {
    cart: state.cart,
    loading: state.loading,
    addToCart,
    updateCartItem,
    removeFromCart,
    clearCart,
    refreshCart,
    getTotalItems,
    getTotalAmount,
    isInCart
  };

  return <CartContext.Provider value={value}>{children}</CartContext.Provider>;
};

// Custom hook to use the cart context
export const useCart = () => {
  const context = useContext(CartContext);
  if (context === undefined) {
    throw new Error('useCart must be used within a CartProvider');
  }
  return context;
}; 