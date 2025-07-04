import api from './api';

export const categoryService = {
  // Get all categories
  getAllCategories: async () => {
    try {
      const response = await api.get('/categories');
      return response.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      throw error;
    }
  },

  // Get category by ID
  getCategoryById: async (id) => {
    try {
      const response = await api.get(`/categories/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching category ${id}:`, error);
      throw error;
    }
  },

  // Get active categories only
  getActiveCategories: async () => {
    try {
      const response = await api.get('/categories/active');
      return response.data;
    } catch (error) {
      console.error('Error fetching active categories:', error);
      throw error;
    }
  }
}; 