import React from 'react';
import { CreditCard, Truck, Shield } from 'lucide-react';
import Button from '../common/Button';
import { useCart } from '../../context/CartContext';

const CartSummary = () => {
  const { cart } = useCart();

  const formatPrice = (price) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  };

  const subtotal = cart.totalAmount || 0;
  const shipping = subtotal > 500 ? 0 : 50;
  const tax = subtotal * 0.08; // 8% tax
  const total = subtotal + shipping + tax;

  return (
    <div className="bg-white rounded-lg shadow-soft p-6 sticky top-24">
      <h2 className="text-xl font-semibold text-gray-900 mb-6">Order Summary</h2>

      {/* Order Details */}
      <div className="space-y-4 mb-6">
        <div className="flex justify-between">
          <span className="text-gray-600">Subtotal ({cart.totalItems || 0} items)</span>
          <span className="font-medium">{formatPrice(subtotal)}</span>
        </div>
        
        <div className="flex justify-between">
          <span className="text-gray-600">Shipping</span>
          <span className="font-medium">
            {shipping === 0 ? (
              <span className="text-green-600">FREE</span>
            ) : (
              formatPrice(shipping)
            )}
          </span>
        </div>
        
        <div className="flex justify-between">
          <span className="text-gray-600">Tax</span>
          <span className="font-medium">{formatPrice(tax)}</span>
        </div>
        
        {shipping === 0 && (
          <div className="text-sm text-green-600 bg-green-50 p-2 rounded">
            🎉 You qualify for free shipping!
          </div>
        )}
        
        {shipping > 0 && (
          <div className="text-sm text-gray-600 bg-gray-50 p-2 rounded">
            Add {formatPrice(500 - subtotal)} more for free shipping
          </div>
        )}
        
        <div className="border-t border-gray-200 pt-4">
          <div className="flex justify-between text-lg font-semibold">
            <span>Total</span>
            <span>{formatPrice(total)}</span>
          </div>
        </div>
      </div>

      {/* Checkout Button */}
      <Button
        size="large"
        className="w-full mb-4"
        disabled={!cart.items || cart.items.length === 0}
      >
        <CreditCard size={20} className="mr-2" />
        Proceed to Checkout
      </Button>

      {/* Features */}
      <div className="space-y-3 text-sm text-gray-600">
        <div className="flex items-center space-x-2">
          <Truck size={16} className="text-green-600" />
          <span>Free shipping on orders over $500</span>
        </div>
        <div className="flex items-center space-x-2">
          <Shield size={16} className="text-blue-600" />
          <span>Secure checkout with SSL encryption</span>
        </div>
        <div className="flex items-center space-x-2">
          <CreditCard size={16} className="text-purple-600" />
          <span>30-day money-back guarantee</span>
        </div>
      </div>

      {/* Payment Methods */}
      <div className="mt-6 pt-6 border-t border-gray-200">
        <p className="text-sm text-gray-600 mb-3">We accept</p>
        <div className="flex space-x-2">
          <div className="bg-gray-100 rounded px-2 py-1 text-xs font-medium">VISA</div>
          <div className="bg-gray-100 rounded px-2 py-1 text-xs font-medium">MC</div>
          <div className="bg-gray-100 rounded px-2 py-1 text-xs font-medium">AMEX</div>
          <div className="bg-gray-100 rounded px-2 py-1 text-xs font-medium">PayPal</div>
        </div>
      </div>
    </div>
  );
};

export default CartSummary; 