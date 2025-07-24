import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import { CartItem, Product } from '@/types';
import { apiService } from '@/services/api';

interface CartState {
  items: CartItem[];
  isLoading: boolean;
  error: string | null;
  
  // Actions
  addToCart: (product: Product, quantity?: number) => Promise<void>;
  updateQuantity: (itemId: number, quantity: number) => Promise<void>;
  removeFromCart: (itemId: number) => Promise<void>;
  clearCart: () => void;
  loadCart: () => Promise<void>;
  
  // Computed values
  getTotalItems: () => number;
  getTotalPrice: () => number;
}

export const useCartStore = create<CartState>()(
  persist(
    (set, get) => ({
      items: [],
      isLoading: false,
      error: null,

      addToCart: async (product: Product, quantity = 1) => {
        set({ isLoading: true, error: null });
        
        try {
          // Check if item already exists in cart
          const existingItemIndex = get().items.findIndex(
            item => item.productId === product.id
          );
          
          if (existingItemIndex >= 0) {
            // Update existing item quantity
            const existingItem = get().items[existingItemIndex];
            await get().updateQuantity(existingItem.id, existingItem.quantity + quantity);
          } else {
            // Add new item to cart
            const newCartItem: Omit<CartItem, 'id'> = {
              cartId: 1, // This should be the user's cart ID
              productId: product.id,
              quantity,
              product,
            };
            
            const result = await apiService.addToCart(newCartItem);
            
            if (result.succeeded) {
              set(state => ({
                items: [...state.items, { ...result.data, product }],
                isLoading: false,
              }));
            } else {
              set({
                isLoading: false,
                error: result.errors || 'Failed to add item to cart',
              });
            }
          }
        } catch (error: any) {
          set({
            isLoading: false,
            error: error.response?.data?.message || 'Failed to add item to cart',
          });
        }
      },

      updateQuantity: async (itemId: number, quantity: number) => {
        if (quantity <= 0) {
          await get().removeFromCart(itemId);
          return;
        }
        
        set({ isLoading: true, error: null });
        
        try {
          const item = get().items.find(item => item.id === itemId);
          if (!item) {
            set({ isLoading: false, error: 'Item not found in cart' });
            return;
          }
          
          const updatedCartItem: Omit<CartItem, 'id'> = {
            cartId: item.cartId,
            productId: item.productId,
            quantity,
          };
          
          const result = await apiService.updateCartItemQuantity(updatedCartItem);
          
          if (result.succeeded) {
            set(state => ({
              items: state.items.map(item =>
                item.id === itemId ? { ...item, quantity } : item
              ),
              isLoading: false,
            }));
          } else {
            set({
              isLoading: false,
              error: result.errors || 'Failed to update cart item',
            });
          }
        } catch (error: any) {
          set({
            isLoading: false,
            error: error.response?.data?.message || 'Failed to update cart item',
          });
        }
      },

      removeFromCart: async (itemId: number) => {
        set({ isLoading: true, error: null });
        
        try {
          const result = await apiService.removeFromCart(itemId);
          
          if (result.succeeded) {
            set(state => ({
              items: state.items.filter(item => item.id !== itemId),
              isLoading: false,
            }));
          } else {
            set({
              isLoading: false,
              error: result.errors || 'Failed to remove item from cart',
            });
          }
        } catch (error: any) {
          set({
            isLoading: false,
            error: error.response?.data?.message || 'Failed to remove item from cart',
          });
        }
      },

      clearCart: () => {
        set({ items: [], error: null });
      },

      loadCart: async () => {
        set({ isLoading: true, error: null });
        
        try {
          const result = await apiService.getCartItems();
          
          if (result.succeeded) {
            set({
              items: result.data,
              isLoading: false,
            });
          } else {
            set({
              isLoading: false,
              error: result.errors || 'Failed to load cart',
            });
          }
        } catch (error: any) {
          set({
            isLoading: false,
            error: error.response?.data?.message || 'Failed to load cart',
          });
        }
      },

      getTotalItems: () => {
        return get().items.reduce((total, item) => total + item.quantity, 0);
      },

      getTotalPrice: () => {
        return get().items.reduce((total, item) => {
          const price = item.product?.price || 0;
          return total + (price * item.quantity);
        }, 0);
      },
    }),
    {
      name: 'cart-storage',
      storage: createJSONStorage(() => localStorage),
      partialize: (state) => ({
        items: state.items,
      }),
    }
  )
);