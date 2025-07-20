import React, { useState, useEffect } from 'react';
import { useAuthStore } from '@/store/authStore';
import { apiService } from '@/services/api';
import { WishList } from '@/types';
import { Heart, ShoppingCart, Trash2, Package } from 'lucide-react';

const WishlistPage: React.FC = () => {
  const { isAuthenticated } = useAuthStore();
  const [wishlistItems, setWishlistItems] = useState<WishList[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (isAuthenticated) {
      loadWishlist();
    } else {
      setLoading(false);
    }
  }, [isAuthenticated]);

  const loadWishlist = async () => {
    try {
      setLoading(true);
      const response = await apiService.getWishlist();
      if (response.succeeded && response.data) {
        setWishlistItems(response.data);
      } else {
        setError(response.errors || 'Failed to load wishlist');
      }
    } catch (err) {
      setError('Failed to load wishlist');
      console.error('Error loading wishlist:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleRemoveFromWishlist = async (productId: number) => {
    try {
      const response = await apiService.removeFromWishlist(productId);
      if (response.succeeded) {
        setWishlistItems(prev => prev.filter(item => item.productId !== productId));
      } else {
        setError(response.errors || 'Failed to remove from wishlist');
      }
    } catch (err) {
      setError('Failed to remove from wishlist');
      console.error('Error removing from wishlist:', err);
    }
  };

  const handleMoveToCart = async (productId: number) => {
    try {
      const cartItem = {
        productId,
        cartId: 0,
        quantity: 1
      };
      
      const response = await apiService.addToCart(cartItem);
      if (response.succeeded) {
        // Remove from wishlist after adding to cart
        await handleRemoveFromWishlist(productId);
        alert('Product moved to cart successfully!');
      } else {
        setError(response.errors || 'Failed to add to cart');
      }
    } catch (err) {
      setError('Failed to add to cart');
      console.error('Error adding to cart:', err);
    }
  };

  if (!isAuthenticated) {
    return (
      <div className="container mx-auto px-4 py-16 text-center">
        <Heart className="h-16 w-16 text-gray-400 mx-auto mb-4" />
        <h1 className="text-2xl font-bold text-gray-900 mb-4">Sign in to view your wishlist</h1>
        <p className="text-gray-600 mb-8">
          You need to be signed in to view and manage your wishlist.
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
            onClick={loadWishlist}
            className="btn btn-primary"
          >
            Try Again
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex items-center justify-between mb-8">
        <h1 className="text-3xl font-bold text-gray-900">My Wishlist</h1>
        <button
          onClick={loadWishlist}
          className="btn btn-outline btn-sm"
        >
          Refresh
        </button>
      </div>

      {wishlistItems.length === 0 ? (
        <div className="text-center py-16">
          <Heart className="h-16 w-16 text-gray-400 mx-auto mb-4" />
          <h2 className="text-xl font-semibold text-gray-900 mb-2">Your wishlist is empty</h2>
          <p className="text-gray-600 mb-8">
            Start adding products to your wishlist to see them here.
          </p>
          <a
            href="/products"
            className="btn btn-primary btn-lg"
          >
            Start Shopping
          </a>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {wishlistItems.map((item) => (
            <div key={item.id} className="bg-white rounded-lg shadow-sm border overflow-hidden">
              {/* Product Image */}
              <div className="aspect-square bg-gray-100 relative">
                <img
                  src="/placeholder-product.jpg"
                  alt={item.product?.name || 'Product'}
                  className="w-full h-full object-cover"
                />
                <button
                  onClick={() => handleRemoveFromWishlist(item.productId)}
                  className="absolute top-2 right-2 p-2 bg-white text-red-500 rounded-full shadow-md hover:bg-red-50 transition-colors"
                >
                  <Trash2 className="h-4 w-4" />
                </button>
              </div>

              {/* Product Info */}
              <div className="p-4">
                <h3 className="font-semibold text-gray-900 mb-2">
                  {item.product?.name || 'Product Name'}
                </h3>
                <p className="text-gray-600 text-sm mb-3">
                  {item.product?.description || 'No description available'}
                </p>
                
                <div className="flex items-center justify-between mb-4">
                  <span className="text-lg font-bold text-primary">
                    ${item.product?.price?.toFixed(2) || '0.00'}
                  </span>
                  <div className="flex items-center space-x-1">
                    <Package className="h-4 w-4 text-gray-400" />
                    <span className="text-sm text-gray-600">
                      {item.product?.stock || 0} in stock
                    </span>
                  </div>
                </div>

                {/* Action Buttons */}
                <div className="space-y-2">
                  <button
                    onClick={() => handleMoveToCart(item.productId)}
                    disabled={!item.product || item.product.stock <= 0}
                    className="w-full btn btn-primary btn-sm flex items-center justify-center space-x-2"
                  >
                    <ShoppingCart className="h-4 w-4" />
                    <span>
                      {!item.product || item.product.stock <= 0 ? 'Out of Stock' : 'Move to Cart'}
                    </span>
                  </button>
                  
                  <a
                    href={`/products/${item.productId}`}
                    className="w-full btn btn-outline btn-sm text-center block"
                  >
                    View Details
                  </a>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default WishlistPage; 