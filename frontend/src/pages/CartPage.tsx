import React, { useState, useEffect } from 'react';
import { useAuthStore } from '@/store/authStore';
import { apiService } from '@/services/api';
import { CartItem, Product } from '@/types';
import { Trash2, Minus, Plus, ShoppingBag, ArrowRight } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

interface CartItemWithProduct extends CartItem {
  product?: Product;
}

const CartPage: React.FC = () => {
  const { isAuthenticated, handleUnauthorized } = useAuthStore();
  const navigate = useNavigate();
  const [cartItems, setCartItems] = useState<CartItemWithProduct[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [updatingItem, setUpdatingItem] = useState<number | null>(null);

  useEffect(() => {
    if (isAuthenticated) {
      loadCart();
    } else {
      setLoading(false);
    }
  }, [isAuthenticated]);

  const loadCart = async () => {
    try {
      setLoading(true);
      const response = await apiService.getMyCart();
      if (response.succeeded && response.data) {
        setCartItems(response.data.cartItems || []);
      } else {
        setError(response.errors || 'Failed to load cart');
      }
    } catch (err: any) {
      if (err.response?.status === 401) {
        handleUnauthorized();
        navigate('/login');
      } else {
        setError('Failed to load cart');
        console.error('Error loading cart:', err);
      }
    } finally {
      setLoading(false);
    }
  };

  const updateQuantity = async (itemId: number, newQuantity: number) => {
    if (newQuantity < 1) return;
    
    try {
      setUpdatingItem(itemId);
      const response = await apiService.updateCartItemQuantity({
        quantity: newQuantity,
        productId: 0,
        cartId: 0
      });
      
      if (response.succeeded) {
        await loadCart(); // Reload cart to get updated data
      } else {
        setError(response.errors || 'Failed to update quantity');
      }
    } catch (err) {
      setError('Failed to update quantity');
      console.error('Error updating quantity:', err);
    } finally {
      setUpdatingItem(null);
    }
  };

  const removeItem = async (itemId: number) => {
    try {
      setUpdatingItem(itemId);
      const response = await apiService.removeFromCart(itemId);
      
      if (response.succeeded) {
        await loadCart(); // Reload cart to get updated data
      } else {
        setError(response.errors || 'Failed to remove item');
      }
    } catch (err) {
      setError('Failed to remove item');
      console.error('Error removing item:', err);
    } finally {
      setUpdatingItem(null);
    }
  };

  const calculateSubtotal = () => {
    return cartItems.reduce((total, item) => {
      const price = item.product?.price || 0;
      return total + (price * item.quantity);
    }, 0);
  };

  const calculateTax = () => {
    return calculateSubtotal() * 0.1; // 10% tax
  };

  const calculateTotal = () => {
    return calculateSubtotal() + calculateTax();
  };

  if (!isAuthenticated) {
    return (
      <div className="container mx-auto px-4 py-16 text-center">
        <ShoppingBag className="h-16 w-16 text-gray-400 mx-auto mb-4" />
        <h1 className="text-2xl font-bold text-gray-900 mb-4">Sign in to view your cart</h1>
        <p className="text-gray-600 mb-8">
          You need to be signed in to view and manage your shopping cart.
        </p>
        <a
          href="/login"
          className="btn btn-primary btn-lg"
        >
          Sign In
        </a>
      </div>
    );
  }

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="flex justify-center items-center h-64">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary"></div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center">
          <div className="text-red-500 mb-4">{error}</div>
          <button
            onClick={loadCart}
            className="btn btn-primary"
          >
            Try Again
          </button>
        </div>
      </div>
    );
  }

  if (cartItems.length === 0) {
    return (
      <div className="container mx-auto px-4 py-16 text-center">
        <ShoppingBag className="h-16 w-16 text-gray-400 mx-auto mb-4" />
        <h1 className="text-2xl font-bold text-gray-900 mb-4">Your cart is empty</h1>
        <p className="text-gray-600 mb-8">
          Looks like you haven't added any items to your cart yet.
        </p>
        <a
          href="/products"
          className="btn btn-primary btn-lg"
        >
          Start Shopping
        </a>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold text-gray-900 mb-8">Shopping Cart</h1>
      
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Cart Items */}
        <div className="lg:col-span-2">
          <div className="bg-white rounded-lg shadow-sm border">
            {cartItems.map((item) => (
              <div key={item.id} className="p-6 border-b last:border-b-0">
                <div className="flex items-center space-x-4">
                  {/* Product Image */}
                  <div className="flex-shrink-0">
                    <img
                      src="/placeholder-product.jpg"
                      alt={item.product?.name || 'Product'}
                      className="w-20 h-20 object-cover rounded-lg"
                    />
                  </div>
                  
                  {/* Product Details */}
                  <div className="flex-1 min-w-0">
                    <h3 className="text-lg font-semibold text-gray-900 truncate">
                      {item.product?.name || 'Product'}
                    </h3>
                    <p className="text-gray-600 text-sm">
                      ${item.product?.price?.toFixed(2) || '0.00'}
                    </p>
                  </div>
                  
                  {/* Quantity Controls */}
                  <div className="flex items-center space-x-2">
                    <button
                      onClick={() => updateQuantity(item.id, item.quantity - 1)}
                      disabled={updatingItem === item.id}
                      className="p-1 rounded-full hover:bg-gray-100 disabled:opacity-50"
                    >
                      <Minus className="h-4 w-4" />
                    </button>
                    <span className="w-12 text-center font-medium">
                      {updatingItem === item.id ? '...' : item.quantity}
                    </span>
                    <button
                      onClick={() => updateQuantity(item.id, item.quantity + 1)}
                      disabled={updatingItem === item.id}
                      className="p-1 rounded-full hover:bg-gray-100 disabled:opacity-50"
                    >
                      <Plus className="h-4 w-4" />
                    </button>
                  </div>
                  
                  {/* Item Total */}
                  <div className="text-right">
                    <p className="text-lg font-semibold text-gray-900">
                      ${((item.product?.price || 0) * item.quantity).toFixed(2)}
                    </p>
                  </div>
                  
                  {/* Remove Button */}
                  <button
                    onClick={() => removeItem(item.id)}
                    disabled={updatingItem === item.id}
                    className="p-2 text-red-500 hover:bg-red-50 rounded-full disabled:opacity-50"
                  >
                    <Trash2 className="h-5 w-5" />
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>
        
        {/* Order Summary */}
        <div className="lg:col-span-1">
          <div className="bg-white rounded-lg shadow-sm border p-6 sticky top-4">
            <h2 className="text-xl font-bold text-gray-900 mb-4">Order Summary</h2>
            
            <div className="space-y-3 mb-6">
              <div className="flex justify-between">
                <span className="text-gray-600">Subtotal</span>
                <span className="font-medium">${calculateSubtotal().toFixed(2)}</span>
              </div>
              <div className="flex justify-between">
                <span className="text-gray-600">Tax (10%)</span>
                <span className="font-medium">${calculateTax().toFixed(2)}</span>
              </div>
              <div className="border-t pt-3">
                <div className="flex justify-between">
                  <span className="text-lg font-bold text-gray-900">Total</span>
                  <span className="text-lg font-bold text-gray-900">
                    ${calculateTotal().toFixed(2)}
                  </span>
                </div>
              </div>
            </div>
            
            <button
              className="w-full btn btn-primary btn-lg flex items-center justify-center space-x-2"
              onClick={() => {
                // TODO: Implement checkout
                alert('Checkout functionality coming soon!');
              }}
            >
              <span>Proceed to Checkout</span>
              <ArrowRight className="h-5 w-5" />
            </button>
            
            <p className="text-xs text-gray-500 mt-4 text-center">
              Free shipping on orders over $50
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CartPage; 