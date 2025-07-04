import React, { useState } from 'react';
import { Star, ShoppingCart, Heart, Share2, ChevronLeft, ChevronRight, Minus, Plus } from 'lucide-react';
import Button from '../common/Button';
import { useCart } from '../../context/CartContext';
import { mapProductImages, getSampleImageByCategory } from '../../utils/productImageUtils';
import toast from 'react-hot-toast';

const ProductDetails = ({ product }) => {
  const [selectedImageIndex, setSelectedImageIndex] = useState(0);
  const [quantity, setQuantity] = useState(1);
  const [isFavorite, setIsFavorite] = useState(false);
  const { addToCart } = useCart();

  const handleAddToCart = () => {
    addToCart(product, quantity);
    toast.success(`${quantity} ${product.name} added to cart!`);
  };

  const formatPrice = (price) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  };

  // Process product images to use local paths
  const localImages = mapProductImages(product.images);
  const images = localImages.length > 0 ? localImages : [
    {
      id: 0,
      imageUrl: getSampleImageByCategory(product.categoryName) || '/images/products/default-product.jpg',
      altText: product.name,
      isPrimary: true,
      displayOrder: 1
    }
  ];
  
  const specifications = product.specifications || [];

  const nextImage = () => {
    setSelectedImageIndex((prev) => (prev + 1) % images.length);
  };

  const prevImage = () => {
    setSelectedImageIndex((prev) => (prev - 1 + images.length) % images.length);
  };

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        {/* Image Gallery */}
        <div className="space-y-4">
          {/* Main Image */}
          <div className="relative aspect-square bg-gray-100 rounded-lg overflow-hidden">
            <img
              src={images[selectedImageIndex]?.imageUrl || '/api/placeholder/600/600'}
              alt={images[selectedImageIndex]?.altText || product.name}
              className="w-full h-full object-cover"
            />
            
            {images.length > 1 && (
              <>
                <button
                  onClick={prevImage}
                  className="absolute left-2 top-1/2 transform -translate-y-1/2 bg-white bg-opacity-80 hover:bg-opacity-100 rounded-full p-2 transition-opacity"
                >
                  <ChevronLeft size={20} />
                </button>
                <button
                  onClick={nextImage}
                  className="absolute right-2 top-1/2 transform -translate-y-1/2 bg-white bg-opacity-80 hover:bg-opacity-100 rounded-full p-2 transition-opacity"
                >
                  <ChevronRight size={20} />
                </button>
              </>
            )}

            {product.stockQuantity <= 5 && product.stockQuantity > 0 && (
              <div className="absolute top-4 left-4 bg-secondary-500 text-white px-3 py-1 rounded text-sm font-semibold">
                Only {product.stockQuantity} left
              </div>
            )}
            {product.stockQuantity === 0 && (
              <div className="absolute top-4 left-4 bg-red-500 text-white px-3 py-1 rounded text-sm font-semibold">
                Out of Stock
              </div>
            )}
          </div>

          {/* Thumbnail Images */}
          {images.length > 1 && (
            <div className="grid grid-cols-4 gap-2">
              {images.map((image, index) => (
                <button
                  key={index}
                  onClick={() => setSelectedImageIndex(index)}
                  className={`aspect-square bg-gray-100 rounded-lg overflow-hidden border-2 ${
                    selectedImageIndex === index ? 'border-primary-600' : 'border-transparent'
                  }`}
                >
                  <img
                    src={image.imageUrl}
                    alt={image.altText}
                    className="w-full h-full object-cover"
                  />
                </button>
              ))}
            </div>
          )}
        </div>

        {/* Product Information */}
        <div className="space-y-6">
          {/* Header */}
          <div>
            <p className="text-sm text-gray-500 mb-2">{product.categoryName}</p>
            <h1 className="text-3xl font-bold text-gray-900 mb-2">{product.name}</h1>
            <p className="text-gray-600">{product.description}</p>
          </div>

          {/* Rating */}
          <div className="flex items-center space-x-4">
            <div className="flex items-center">
              {[...Array(5)].map((_, i) => (
                <Star
                  key={i}
                  size={20}
                  className={`${i < 4 ? 'text-yellow-400 fill-current' : 'text-gray-300'}`}
                />
              ))}
            </div>
            <span className="text-sm text-gray-600">(24 reviews)</span>
            <button className="text-sm text-primary-600 hover:text-primary-700">
              Write a review
            </button>
          </div>

          {/* Price */}
          <div className="border-t border-b border-gray-200 py-6">
            <div className="flex items-center justify-between">
              <span className="text-3xl font-bold text-gray-900">
                {formatPrice(product.price)}
              </span>
              <span className="text-sm text-gray-500">SKU: {product.sku}</span>
            </div>
          </div>

          {/* Quantity and Add to Cart */}
          <div className="space-y-4">
            <div className="flex items-center space-x-4">
              <span className="text-sm font-medium text-gray-700">Quantity:</span>
              <div className="flex items-center border border-gray-300 rounded-lg">
                <button
                  onClick={() => setQuantity(Math.max(1, quantity - 1))}
                  className="p-2 hover:bg-gray-50"
                  disabled={quantity <= 1}
                >
                  <Minus size={16} />
                </button>
                <span className="px-4 py-2 text-center min-w-[60px]">{quantity}</span>
                <button
                  onClick={() => setQuantity(Math.min(product.stockQuantity, quantity + 1))}
                  className="p-2 hover:bg-gray-50"
                  disabled={quantity >= product.stockQuantity}
                >
                  <Plus size={16} />
                </button>
              </div>
              <span className="text-sm text-gray-500">
                {product.stockQuantity} available
              </span>
            </div>

            <div className="flex space-x-4">
              <Button
                onClick={handleAddToCart}
                className="flex-1"
                size="large"
                disabled={product.stockQuantity === 0}
              >
                <ShoppingCart size={20} className="mr-2" />
                Add to Cart
              </Button>
              <Button
                variant="outline"
                size="large"
                onClick={() => setIsFavorite(!isFavorite)}
              >
                <Heart 
                  size={20} 
                  className={isFavorite ? 'fill-current text-red-500' : ''} 
                />
              </Button>
              <Button variant="outline" size="large">
                <Share2 size={20} />
              </Button>
            </div>
          </div>

          {/* Specifications */}
          {specifications.length > 0 && (
            <div className="border-t border-gray-200 pt-6">
              <h3 className="text-lg font-semibold text-gray-900 mb-4">Specifications</h3>
              <dl className="space-y-3">
                {specifications.map((spec) => (
                  <div key={spec.id} className="flex">
                    <dt className="flex-shrink-0 w-32 text-sm font-medium text-gray-500">
                      {spec.name}:
                    </dt>
                    <dd className="text-sm text-gray-900">{spec.value}</dd>
                  </div>
                ))}
              </dl>
            </div>
          )}

          {/* Features */}
          <div className="border-t border-gray-200 pt-6">
            <h3 className="text-lg font-semibold text-gray-900 mb-4">Features</h3>
            <ul className="space-y-2">
              <li className="flex items-center text-sm text-gray-600">
                <span className="w-2 h-2 bg-primary-600 rounded-full mr-3"></span>
                Free shipping on orders over $500
              </li>
              <li className="flex items-center text-sm text-gray-600">
                <span className="w-2 h-2 bg-primary-600 rounded-full mr-3"></span>
                30-day return policy
              </li>
              <li className="flex items-center text-sm text-gray-600">
                <span className="w-2 h-2 bg-primary-600 rounded-full mr-3"></span>
                Professional assembly available
              </li>
              <li className="flex items-center text-sm text-gray-600">
                <span className="w-2 h-2 bg-primary-600 rounded-full mr-3"></span>
                1-year warranty included
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductDetails; 