import React from 'react';
import { Link } from 'react-router-dom';
import { Star, ShoppingCart, Eye } from 'lucide-react';
import Button from '../common/Button';
import { useCart } from '../../context/CartContext';
import { mapProductImages, getSampleImageByCategory } from '../../utils/productImageUtils';
import toast from 'react-hot-toast';

const ProductCard = ({ product }) => {
  const { addToCart } = useCart();

  const handleAddToCart = (e) => {
    e.preventDefault();
    e.stopPropagation();
    addToCart(product, 1);
    toast.success(`${product.name} added to cart!`);
  };

  const formatPrice = (price) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  };

  // Process product images to use local paths
  const localImages = mapProductImages(product.images);
  const primaryImage = localImages?.find(img => img.isPrimary) || localImages?.[0];
  
  // Fallback to category-based sample image if no product images available
  const displayImage = primaryImage?.imageUrl || getSampleImageByCategory(product.categoryName) || '/images/products/default-product.jpg';

  return (
    <div className="group relative bg-white rounded-lg shadow-soft hover:shadow-lg transition-shadow duration-300 overflow-hidden">
      <Link to={`/products/${product.id}`}>
        {/* Image */}
        <div className="aspect-w-1 aspect-h-1 w-full overflow-hidden bg-gray-200">
          <img
            src={displayImage}
            alt={primaryImage?.altText || product.name}
            className="h-64 w-full object-cover object-center group-hover:scale-105 transition-transform duration-300"
          />
          {product.stockQuantity <= 5 && product.stockQuantity > 0 && (
            <div className="absolute top-2 left-2 bg-secondary-500 text-white px-2 py-1 rounded text-xs font-semibold">
              Only {product.stockQuantity} left
            </div>
          )}
          {product.stockQuantity === 0 && (
            <div className="absolute top-2 left-2 bg-red-500 text-white px-2 py-1 rounded text-xs font-semibold">
              Out of Stock
            </div>
          )}
        </div>

        {/* Product Info */}
        <div className="p-4">
          <div className="mb-2">
            <p className="text-sm text-gray-500">{product.categoryName}</p>
            <h3 className="text-lg font-semibold text-gray-900 group-hover:text-primary-600 transition-colors">
              {product.name}
            </h3>
          </div>
          
          <p className="text-sm text-gray-600 mb-3 line-clamp-2">
            {product.description}
          </p>

          {/* Rating */}
          <div className="flex items-center mb-3">
            <div className="flex items-center">
              {[...Array(5)].map((_, i) => (
                <Star
                  key={i}
                  size={16}
                  className={`${i < 4 ? 'text-yellow-400 fill-current' : 'text-gray-300'}`}
                />
              ))}
            </div>
            <span className="ml-2 text-sm text-gray-600">(24 reviews)</span>
          </div>

          {/* Price */}
          <div className="flex items-center justify-between mb-4">
            <span className="text-2xl font-bold text-gray-900">
              {formatPrice(product.price)}
            </span>
            <span className="text-sm text-gray-500">SKU: {product.sku}</span>
          </div>
        </div>
      </Link>

      {/* Action Buttons */}
      <div className="absolute inset-x-0 bottom-0 bg-white bg-opacity-90 backdrop-blur-sm p-4 transform translate-y-full group-hover:translate-y-0 transition-transform duration-300">
        <div className="flex space-x-2">
          <Button
            onClick={handleAddToCart}
            className="flex-1"
            disabled={product.stockQuantity === 0}
          >
            <ShoppingCart size={16} className="mr-2" />
            Add to Cart
          </Button>
          <Link to={`/products/${product.id}`}>
            <Button variant="outline" size="medium">
              <Eye size={16} />
            </Button>
          </Link>
        </div>
      </div>
    </div>
  );
};

export default ProductCard; 