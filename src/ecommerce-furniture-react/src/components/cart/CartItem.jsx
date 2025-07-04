import React from 'react';
import { Minus, Plus, Trash2 } from 'lucide-react';
import Button from '../common/Button';
import { useCart } from '../../context/CartContext';
import toast from 'react-hot-toast';

const CartItem = ({ item }) => {
  const { updateCartItem, removeFromCart } = useCart();

  const handleQuantityChange = (newQuantity) => {
    if (newQuantity < 1) {
      handleRemove();
      return;
    }
    updateCartItem(item.id, newQuantity);
  };

  const handleRemove = () => {
    removeFromCart(item.id);
    toast.success('Item removed from cart');
  };

  const formatPrice = (price) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  };

  const primaryImage = item.productImageUrl || '/api/placeholder/200/200';

  return (
    <div className="flex items-center space-x-4 p-4 bg-white rounded-lg shadow-sm border border-gray-200">
      {/* Product Image */}
      <div className="flex-shrink-0">
        <img
          src={primaryImage}
          alt={item.productName}
          className="w-20 h-20 object-cover rounded-lg"
        />
      </div>

      {/* Product Details */}
      <div className="flex-1 min-w-0">
        <h3 className="text-lg font-semibold text-gray-900 truncate">
          {item.productName}
        </h3>
        <p className="text-sm text-gray-500 mb-1">SKU: {item.productSKU}</p>
        <p className="text-lg font-bold text-gray-900">
          {formatPrice(item.unitPrice)}
        </p>
      </div>

      {/* Quantity Controls */}
      <div className="flex items-center space-x-3">
        <div className="flex items-center border border-gray-300 rounded-lg">
          <button
            onClick={() => handleQuantityChange(item.quantity - 1)}
            className="p-2 hover:bg-gray-50 transition-colors"
            disabled={item.quantity <= 1}
          >
            <Minus size={16} />
          </button>
          <span className="px-4 py-2 text-center min-w-[60px] font-medium">
            {item.quantity}
          </span>
          <button
            onClick={() => handleQuantityChange(item.quantity + 1)}
            className="p-2 hover:bg-gray-50 transition-colors"
          >
            <Plus size={16} />
          </button>
        </div>

        {/* Total Price */}
        <div className="text-right min-w-[100px]">
          <p className="text-lg font-bold text-gray-900">
            {formatPrice(item.totalPrice)}
          </p>
        </div>

        {/* Remove Button */}
        <Button
          variant="ghost"
          size="small"
          onClick={handleRemove}
          className="text-red-600 hover:text-red-700 hover:bg-red-50"
        >
          <Trash2 size={16} />
        </Button>
      </div>
    </div>
  );
};

export default CartItem; 