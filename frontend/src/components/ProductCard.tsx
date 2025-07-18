import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { Heart, ShoppingCart, Star, Eye } from 'lucide-react';
import { Product } from '@/types';
import { useCartStore } from '@/store/cartStore';
import { useAuthStore } from '@/store/authStore';
import { formatPrice, truncateText } from '@/lib/utils';

interface ProductCardProps {
  product: Product;
  className?: string;
}

const ProductCard: React.FC<ProductCardProps> = ({ product, className }) => {
  const [isWishlisted, setIsWishlisted] = useState(false);
  const [imageError, setImageError] = useState(false);
  const { addToCart, isLoading } = useCartStore();
  const { isAuthenticated } = useAuthStore();

  const handleAddToCart = async (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    
    if (!isAuthenticated) {
      // Redirect to login or show login modal
      return;
    }

    try {
      await addToCart(product, 1);
    } catch (error) {
      console.error('Failed to add to cart:', error);
    }
  };

  const handleWishlistToggle = (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    
    if (!isAuthenticated) {
      // Redirect to login or show login modal
      return;
    }

    setIsWishlisted(!isWishlisted);
  };

  const placeholderImage = `https://via.placeholder.com/300x300/f3f4f6/9ca3af?text=${encodeURIComponent(product.name)}`;

  return (
    <div className={`group relative bg-white rounded-lg border border-gray-200 shadow-sm hover:shadow-lg transition-all duration-300 overflow-hidden ${className}`}>
      <Link to={`/products/${product.id}`} className="block">
        {/* Product Image */}
        <div className="relative aspect-square overflow-hidden bg-gray-100">
          <img
            src={imageError ? placeholderImage : placeholderImage}
            alt={product.name}
            className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
            onError={() => setImageError(true)}
          />
          
          {/* Stock Status */}
          {product.stock <= 0 && (
            <div className="absolute inset-0 bg-black/50 flex items-center justify-center">
              <span className="text-white font-medium">Out of Stock</span>
            </div>
          )}
          
          {/* Quick Actions */}
          <div className="absolute top-2 right-2 flex flex-col space-y-2 opacity-0 group-hover:opacity-100 transition-opacity">
            <button
              onClick={handleWishlistToggle}
              className={`p-2 rounded-full ${
                isWishlisted 
                  ? 'bg-red-500 text-white' 
                  : 'bg-white text-gray-600 hover:text-red-500'
              } shadow-md transition-colors`}
            >
              <Heart className={`h-4 w-4 ${isWishlisted ? 'fill-current' : ''}`} />
            </button>
            
            <Link
              to={`/products/${product.id}`}
              className="p-2 bg-white text-gray-600 hover:text-primary rounded-full shadow-md transition-colors"
            >
              <Eye className="h-4 w-4" />
            </Link>
          </div>

          {/* Sale Badge */}
          {product.price < 100 && (
            <div className="absolute top-2 left-2">
              <span className="bg-red-500 text-white text-xs font-medium px-2 py-1 rounded">
                Sale
              </span>
            </div>
          )}
        </div>

        {/* Product Info */}
        <div className="p-4">
          <h3 className="text-sm font-medium text-gray-900 mb-1 group-hover:text-primary transition-colors">
            {truncateText(product.name, 50)}
          </h3>
          
          <p className="text-sm text-gray-500 mb-2">
            {truncateText(product.description, 80)}
          </p>

          {/* Rating */}
          <div className="flex items-center mb-2">
            <div className="flex items-center">
              {[...Array(5)].map((_, i) => (
                <Star
                  key={i}
                  className={`h-4 w-4 ${
                    i < 4 ? 'text-yellow-400 fill-current' : 'text-gray-300'
                  }`}
                />
              ))}
            </div>
            <span className="text-sm text-gray-500 ml-2">(4.0)</span>
          </div>

          {/* Price */}
          <div className="flex items-center justify-between mb-3">
            <div className="flex items-center space-x-2">
              <span className="text-lg font-semibold text-gray-900">
                {formatPrice(product.price)}
              </span>
              {product.price < 100 && (
                <span className="text-sm text-gray-500 line-through">
                  {formatPrice(product.price * 1.2)}
                </span>
              )}
            </div>
            
            {product.stock <= 10 && product.stock > 0 && (
              <span className="text-xs text-orange-600 font-medium">
                Only {product.stock} left
              </span>
            )}
          </div>
        </div>
      </Link>

      {/* Add to Cart Button */}
      <div className="p-4 pt-0">
        <button
          onClick={handleAddToCart}
          disabled={product.stock <= 0 || isLoading}
          className={`w-full btn ${
            product.stock <= 0 
              ? 'btn-secondary opacity-50 cursor-not-allowed' 
              : 'btn-primary'
          } btn-sm`}
        >
          {isLoading ? (
            <div className="flex items-center justify-center">
              <div className="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin mr-2"></div>
              Adding...
            </div>
          ) : (
            <div className="flex items-center justify-center">
              <ShoppingCart className="h-4 w-4 mr-2" />
              {product.stock <= 0 ? 'Out of Stock' : 'Add to Cart'}
            </div>
          )}
        </button>
      </div>
    </div>
  );
};

export default ProductCard;