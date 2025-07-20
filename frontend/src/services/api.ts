import axios, { AxiosInstance, AxiosResponse } from 'axios';
import {
  ApiResult,
  Product,
  Category,
  LoginRequest,
  RegisterRequest,
  LoginResponse,
  RegisterResponse,
  CartItem,
  Review,
  WishList,
  Order,
  Coupon,
  ShippingAddress,
} from '@/types';
import { mockApiService } from './mockData';

class ApiService {
  private api: AxiosInstance;
  private authApi: AxiosInstance;
  private baseURL = 'https://e-commerce-nr8a.onrender.com';
  private useMockData = false; // Set to true to force mock data

  constructor() {
    // Main API instance for /api/* endpoints
    this.api = axios.create({
      baseURL: `${this.baseURL}/api`,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Separate instance for auth endpoints
    this.authApi = axios.create({
      baseURL: this.baseURL,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Request interceptor to add auth token for both APIs
    const addAuthInterceptor = (instance: AxiosInstance) => {
      instance.interceptors.request.use(
        (config) => {
          const token = localStorage.getItem('accessToken');
          if (token) {
            config.headers.Authorization = `Bearer ${token}`;
          }
          return config;
        },
        (error) => {
          return Promise.reject(error);
        }
      );

      // Response interceptor for error handling
      instance.interceptors.response.use(
        (response) => response,
        (error) => {
          if (error.response?.status === 401) {
            // Handle unauthorized - redirect to login
            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');
            window.location.href = '/login';
          }
          return Promise.reject(error);
        }
      );
    };

    addAuthInterceptor(this.api);
    addAuthInterceptor(this.authApi);
  }

  // Method to set auth token after login
  setAuthToken(token: string) {
    localStorage.setItem('accessToken', token);
    // Update the default headers for both API instances
    this.api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    this.authApi.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  }

  // Method to clear auth token on logout
  clearAuthToken() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    delete this.api.defaults.headers.common['Authorization'];
    delete this.authApi.defaults.headers.common['Authorization'];
  }

  // Authentication endpoints
  async login(credentials: LoginRequest): Promise<ApiResult<LoginResponse>> {
    const response: AxiosResponse<ApiResult<LoginResponse>> = await this.authApi.post('/auth/login', credentials);
    
    // If login successful, store the access token
    if (response.data.succeeded && response.data.data.accessToken) {
      this.setAuthToken(response.data.data.accessToken);
    }
    
    return response.data;
  }

  async register(userData: RegisterRequest): Promise<ApiResult<RegisterResponse>> {
    const response: AxiosResponse<ApiResult<RegisterResponse>> = await this.authApi.post('/auth/register', userData);
    return response.data;
  }

  // Product endpoints with fallback to mock data
  async getProducts(): Promise<ApiResult<Product[]>> {
    if (this.useMockData) {
      return mockApiService.getProducts();
    }

    try {
      const response: AxiosResponse<ApiResult<Product[]>> = await this.api.get('/product');
      return response.data;
    } catch (error: any) {
      console.warn('Real API failed, using mock data:', error.message);
      return mockApiService.getProducts();
    }
  }

  async getProduct(id: number): Promise<ApiResult<Product>> {
    try {
      const response: AxiosResponse<ApiResult<Product>> = await this.api.get(`/product/${id}`);
      return response.data;
    } catch (error: any) {
      // Fallback to mock data
      const mockProducts = await mockApiService.getProducts();
      const product = mockProducts.data.find(p => p.id === id);
      if (product) {
        return { succeeded: true, data: product, errors: "" };
      }
      throw error;
    }
  }

  async createProduct(product: Omit<Product, 'id'>): Promise<ApiResult<Product>> {
    const response: AxiosResponse<ApiResult<Product>> = await this.api.post('/product', product);
    return response.data;
  }

  async updateProduct(id: number, product: Omit<Product, 'id'>): Promise<ApiResult<Product>> {
    const response: AxiosResponse<ApiResult<Product>> = await this.api.put(`/product/${id}`, product);
    return response.data;
  }

  async deleteProduct(id: number): Promise<ApiResult<boolean>> {
    const response: AxiosResponse<ApiResult<boolean>> = await this.api.delete(`/product/${id}`);
    return response.data;
  }

  // Category endpoints with fallback to mock data
  async getCategories(): Promise<ApiResult<Category[]>> {
    if (this.useMockData) {
      return mockApiService.getCategories();
    }

    try {
      const response: AxiosResponse<ApiResult<Category[]>> = await this.api.get('/category');
      return response.data;
    } catch (error: any) {
      console.warn('Real API failed, using mock data:', error.message);
      return mockApiService.getCategories();
    }
  }

  async getCategory(id: number): Promise<ApiResult<Category>> {
    try {
      const response: AxiosResponse<ApiResult<Category>> = await this.api.get(`/category/${id}`);
      return response.data;
    } catch (error: any) {
      // Fallback to mock data
      const mockCategories = await mockApiService.getCategories();
      const category = mockCategories.data.find(c => c.id === id);
      if (category) {
        return { succeeded: true, data: category, errors: "" };
      }
      throw error;
    }
  }

  async createCategory(category: Omit<Category, 'id'>): Promise<ApiResult<Category>> {
    const response: AxiosResponse<ApiResult<Category>> = await this.api.post('/category', category);
    return response.data;
  }

  async updateCategory(id: number, category: Omit<Category, 'id'>): Promise<ApiResult<Category>> {
    const response: AxiosResponse<ApiResult<Category>> = await this.api.put(`/category/${id}`, category);
    return response.data;
  }

  async deleteCategory(id: number): Promise<ApiResult<boolean>> {
    const response: AxiosResponse<ApiResult<boolean>> = await this.api.delete(`/category/${id}`);
    return response.data;
  }

  // Cart endpoints
  async getCartItems(): Promise<ApiResult<CartItem[]>> {
    const response: AxiosResponse<ApiResult<CartItem[]>> = await this.api.get('/cartitem');
    return response.data;
  }

  async addToCart(cartItem: Omit<CartItem, 'id'>): Promise<ApiResult<CartItem>> {
    const response: AxiosResponse<ApiResult<CartItem>> = await this.api.post('/cartitem', cartItem);
    return response.data;
  }

  async updateCartItem(id: number, cartItem: Omit<CartItem, 'id'>): Promise<ApiResult<CartItem>> {
    const response: AxiosResponse<ApiResult<CartItem>> = await this.api.put(`/cartitem/${id}`, cartItem);
    return response.data;
  }

  async removeFromCart(id: number): Promise<ApiResult<boolean>> {
    const response: AxiosResponse<ApiResult<boolean>> = await this.api.delete(`/cartitem/${id}`);
    return response.data;
  }

  // Review endpoints
  async getProductReviews(productId: number): Promise<ApiResult<Review[]>> {
    const response: AxiosResponse<ApiResult<Review[]>> = await this.api.get(`/review?productId=${productId}`);
    return response.data;
  }

  async createReview(review: Omit<Review, 'id' | 'isApproved' | 'isDeleted'>): Promise<ApiResult<Review>> {
    const response: AxiosResponse<ApiResult<Review>> = await this.api.post('/review', review);
    return response.data;
  }

  // Wishlist endpoints
  async getWishlist(): Promise<ApiResult<WishList[]>> {
    const response: AxiosResponse<ApiResult<WishList[]>> = await this.api.get('/wishlist');
    return response.data;
  }

  async addToWishlist(wishlistItem: Omit<WishList, 'id'>): Promise<ApiResult<WishList>> {
    const response: AxiosResponse<ApiResult<WishList>> = await this.api.post('/wishlist', wishlistItem);
    return response.data;
  }

  async removeFromWishlist(id: number): Promise<ApiResult<boolean>> {
    const response: AxiosResponse<ApiResult<boolean>> = await this.api.delete(`/wishlist/${id}`);
    return response.data;
  }

  // Order endpoints
  async getOrders(): Promise<ApiResult<Order[]>> {
    const response: AxiosResponse<ApiResult<Order[]>> = await this.api.get('/order');
    return response.data;
  }

  async getOrder(id: number): Promise<ApiResult<Order>> {
    const response: AxiosResponse<ApiResult<Order>> = await this.api.get(`/order/${id}`);
    return response.data;
  }

  async createOrder(order: Omit<Order, 'id' | 'orderDate' | 'isDeleted'>): Promise<ApiResult<Order>> {
    const response: AxiosResponse<ApiResult<Order>> = await this.api.post('/order', order);
    return response.data;
  }

  // Coupon endpoints
  async getCoupons(): Promise<ApiResult<Coupon[]>> {
    const response: AxiosResponse<ApiResult<Coupon[]>> = await this.api.get('/coupon');
    return response.data;
  }

  async validateCoupon(code: string): Promise<ApiResult<Coupon>> {
    const response: AxiosResponse<ApiResult<Coupon>> = await this.api.get(`/coupon/validate/${code}`);
    return response.data;
  }

  // Shipping Address endpoints
  async getShippingAddresses(): Promise<ApiResult<ShippingAddress[]>> {
    const response: AxiosResponse<ApiResult<ShippingAddress[]>> = await this.api.get('/shippingaddress');
    return response.data;
  }

  async createShippingAddress(address: Omit<ShippingAddress, 'id' | 'isDeleted'>): Promise<ApiResult<ShippingAddress>> {
    const response: AxiosResponse<ApiResult<ShippingAddress>> = await this.api.post('/shippingaddress', address);
    return response.data;
  }

  async updateShippingAddress(id: number, address: Omit<ShippingAddress, 'id' | 'isDeleted'>): Promise<ApiResult<ShippingAddress>> {
    const response: AxiosResponse<ApiResult<ShippingAddress>> = await this.api.put(`/shippingaddress/${id}`, address);
    return response.data;
  }

  async deleteShippingAddress(id: number): Promise<ApiResult<boolean>> {
    const response: AxiosResponse<ApiResult<boolean>> = await this.api.delete(`/shippingaddress/${id}`);
    return response.data;
  }
}

export const apiService = new ApiService();