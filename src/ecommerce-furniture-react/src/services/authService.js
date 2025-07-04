import apiClient from './api';

export const authService = {
  // Login user
  login: async (username, password) => {
    try {
      alert(username + " " + password); 
      const response = await apiClient.post('/auth/login', {
        username,
        password
      });
      
      if (response.data.success) { 
        // Store token in localStorage
        localStorage.setItem('token', response.data.token);
        localStorage.setItem('username', response.data.username);
        return response.data;
      } else {
        throw new Error(response.data.message || 'Login failed');
      }
    } catch (error) {
      console.error('Login error:', error);
      throw error;
    }
  },

  // Logout user
  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
  },

  // Get current user
  getCurrentUser: () => {
    const username = localStorage.getItem('username');
    return username;
  },

  // Check if user is authenticated
  isAuthenticated: () => {
    const token = localStorage.getItem('token');
    return !!token;
  },

  // Get auth token
  getToken: () => {
    return localStorage.getItem('token');
  },

  // Validate token with server
  validateToken: async () => {
    try {
      const token = localStorage.getItem('token');
      if (!token) return false;

      const response = await apiClient.post('/auth/validate', null, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      
      return response.data;
    } catch (error) {
      console.error('Token validation error:', error);
      return false;
    }
  },

  // Get current user from server
  getCurrentUserFromServer: async () => {
    try {
      const token = localStorage.getItem('token');
      if (!token) return null;

      const response = await apiClient.get('/auth/user', {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      
      return response.data.username;
    } catch (error) {
      console.error('Get current user error:', error);
      return null;
    }
  }
}; 