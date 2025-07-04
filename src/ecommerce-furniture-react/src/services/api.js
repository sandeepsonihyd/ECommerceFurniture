import axios from 'axios';

// Create axios instance with base configuration
const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5009/api';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor
apiClient.interceptors.request.use(
  (config) => {
    console.log(`Making ${config.method?.toUpperCase()} request to: ${config.url}`);
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor
apiClient.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    console.error('API Error:', error.response?.data || error.message);
    return Promise.reject(error);
  }
);

// Product Service
export const productService = {
  // Get all products with optional filters
  getAll: (params = {}) => {
    return apiClient.get('/products', { params });
  },

  // Get product by ID
  getById: (id) => {
    return apiClient.get(`/products/${id}`);
  },

  // Get products by category
  getByCategory: (categoryId) => {
    return apiClient.get(`/products/category/${categoryId}`);
  },

  // Search products
  search: (term) => {
    return apiClient.get(`/products/search`, { params: { term } });
  },

  // Get featured products
  getFeatured: () => {
    return apiClient.get('/products/featured');
  }
};

// Category Service
export const categoryService = {
  // Get all categories
  getAll: () => {
    return apiClient.get('/categories');
  },

  // Get category by ID
  getById: (id) => {
    return apiClient.get(`/categories/${id}`);
  },

  // Get active categories only
  getActive: () => {
    return apiClient.get('/categories/active');
  }
};

// Cart Service
export const cartService = {
  // Get cart by session ID
  getCart: (sessionId) => {
    return apiClient.get(`/cart/${sessionId}`);
  },

  // Add item to cart
  addToCart: (data) => {
    return apiClient.post('/cart/add', data);
  },

  // Update cart item quantity
  updateCartItem: (data) => {
    return apiClient.put('/cart/update', data);
  },

  // Remove item from cart
  removeFromCart: (cartItemId) => {
    return apiClient.delete(`/cart/remove/${cartItemId}`);
  },

  // Clear entire cart
  clearCart: (sessionId) => {
    return apiClient.delete(`/cart/clear/${sessionId}`);
  }
};

// Export the configured axios instance for custom requests
export default apiClient; 