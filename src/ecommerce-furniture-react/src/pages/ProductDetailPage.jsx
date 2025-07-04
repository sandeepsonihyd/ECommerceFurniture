import React, { useState, useEffect } from 'react';
import { useParams, Navigate } from 'react-router-dom';
import ProductDetails from '../components/product/ProductDetails';
import Loading from '../components/common/Loading';
import { productService } from '../services/api';
import toast from 'react-hot-toast';

const ProductDetailPage = () => {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);

  useEffect(() => {
    const fetchProduct = async () => {
      try {
        setLoading(true);
        const response = await productService.getById(id);
        setProduct(response.data);
      } catch (error) {
        console.error('Error fetching product:', error);
        setError(true);
        toast.error('Product not found');
      } finally {
        setLoading(false);
      }
    };

    if (id) {
      fetchProduct();
    }
  }, [id]);

  if (loading) {
    return <Loading size="large" text="Loading product..." />;
  }

  if (error || !product) {
    return <Navigate to="/products" replace />;
  }

  return <ProductDetails product={product} />;
};

export default ProductDetailPage; 