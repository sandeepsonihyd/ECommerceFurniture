import React, { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import ProductList from '../components/product/ProductList';
import { productService } from '../services/api';
import toast from 'react-hot-toast';

const ProductsPage = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchParams] = useSearchParams();

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        setLoading(true);
        const categoryId = searchParams.get('categoryId');
        const searchTerm = searchParams.get('search');
        
        let response;
        if (categoryId) {
          response = await productService.getByCategory(categoryId);
        } else if (searchTerm) {
          response = await productService.search(searchTerm);
        } else {
          response = await productService.getAll();
        }
        
        setProducts(response.data);
      } catch (error) {
        console.error('Error fetching products:', error);
        toast.error('Failed to load products');
      } finally {
        setLoading(false);
      }
    };

    fetchProducts();
  }, [searchParams]);

  const getPageTitle = () => {
    const categoryId = searchParams.get('categoryId');
    const searchTerm = searchParams.get('search');
    
    if (searchTerm) {
      return `Search Results for "${searchTerm}"`;
    } else if (categoryId) {
      return 'Category Products';
    }
    return 'All Products';
  };

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <ProductList 
        products={products} 
        loading={loading} 
        title={getPageTitle()}
      />
    </div>
  );
};

export default ProductsPage; 