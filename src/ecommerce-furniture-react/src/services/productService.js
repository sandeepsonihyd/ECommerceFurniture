import api from './api';
import { processProductForLocalImages, processProductsForLocalImages } from '../utils/productImageUtils';

export const productService = {
  // Get all products
  getAllProducts: async () => {
    try {
      const response = await api.get('/products');
      return processProductsForLocalImages(response.data);
    } catch (error) {
      console.error('Error fetching products:', error);
      throw error;
    }
  },

  // Get product by ID
  getProductById: async (id) => {
    try {
      const response = await api.get(`/products/${id}`);
      return processProductForLocalImages(response.data);
    } catch (error) {
      console.error(`Error fetching product ${id}:`, error);
      throw error;
    }
  },

  // Get products by category
  getProductsByCategory: async (categoryId) => {
    try {
      const response = await api.get(`/products/category/${categoryId}`);
      return processProductsForLocalImages(response.data);
    } catch (error) {
      console.error(`Error fetching products for category ${categoryId}:`, error);
      throw error;
    }
  },

  // Search products
  searchProducts: async (searchTerm) => {
    try {
      const response = await api.get(`/products/search?term=${encodeURIComponent(searchTerm)}`);
      return processProductsForLocalImages(response.data);
    } catch (error) {
      console.error(`Error searching products with term "${searchTerm}":`, error);
      throw error;
    }
  },

  // Get featured products
  getFeaturedProducts: async () => {
    try {
      const response = await api.get('/products/featured');
      return processProductsForLocalImages(response.data);
    } catch (error) {
      console.error('Error fetching featured products:', error);
      throw error;
    }
  },

  // Get products with pagination and filters
  getProductsPaged: async (filters = {}) => {
    try {
      const params = new URLSearchParams();
      
      if (filters.categoryId) params.append('categoryId', filters.categoryId);
      if (filters.searchTerm) params.append('searchTerm', filters.searchTerm);
      if (filters.minPrice) params.append('minPrice', filters.minPrice);
      if (filters.maxPrice) params.append('maxPrice', filters.maxPrice);
      if (filters.pageNumber) params.append('pageNumber', filters.pageNumber);
      if (filters.pageSize) params.append('pageSize', filters.pageSize);

      const response = await api.get(`/products?${params.toString()}`);
      
      // Extract pagination info from headers
      const totalCount = parseInt(response.headers['x-total-count'] || '0');
      const pageNumber = parseInt(response.headers['x-page-number'] || '1');
      const pageSize = parseInt(response.headers['x-page-size'] || '12');
      const totalPages = parseInt(response.headers['x-total-pages'] || '1');

      return {
        items: processProductsForLocalImages(response.data),
        totalCount,
        pageNumber,
        pageSize,
        totalPages,
        hasNextPage: pageNumber < totalPages,
        hasPreviousPage: pageNumber > 1
      };
    } catch (error) {
      console.error('Error fetching paged products:', error);
      throw error;
    }
  }
}; 