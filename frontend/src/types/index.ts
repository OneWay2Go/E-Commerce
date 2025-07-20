// API Response wrapper
export interface ApiResult<T> {
  data: T;
  succeeded: boolean;
  errors: string;
}

// Product types
export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  categoryId: number;
}

// Category types
export interface Category {
  id: number;
  name: string;
  description: string;
  parentId: number;
}

// Authentication types
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  phoneNumber: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  role: string;
}

export interface RegisterResponse {
  email: string;
  message: string;
}

// User types
export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
}

// Cart types
export interface Cart {
  id: number;
  userId: number;
  isDeleted: boolean;
}

export interface CartItem {
  id: number;
  cartId: number;
  productId: number;
  quantity: number;
  product?: Product;
}

// Review types
export interface Review {
  id: number;
  productId: number;
  userId: number;
  rating: number;
  comment: string;
  isApproved: boolean;
  isDeleted: boolean;
}

// Wishlist types
export interface WishList {
  id: number;
  userId: number;
  productId: number;
  product?: Product;
}

// Order types
export interface Order {
  id: number;
  userId: number;
  couponId?: number;
  orderDate: string;
  status: OrderStatus;
  totalAmount: number;
  shippingAddressId: number;
  notes: string;
  isDeleted: boolean;
}

export interface OrderItem {
  id: number;
  orderId: number;
  productId: number;
  quantity: number;
  price: number;
}

// Enums
export enum OrderStatus {
  Pending = 'Pending',
  Processing = 'Processing',
  Shipped = 'Shipped',
  Delivered = 'Delivered',
  Cancelled = 'Cancelled'
}

export enum PaymentMethod {
  CreditCard = 'CreditCard',
  DebitCard = 'DebitCard',
  PayPal = 'PayPal',
  Cash = 'Cash'
}

export enum PaymentStatus {
  Pending = 'Pending',
  Completed = 'Completed',
  Failed = 'Failed',
  Refunded = 'Refunded'
}

// Coupon types
export interface Coupon {
  id: number;
  code: string;
  description: string;
  discountType: DiscountType;
  discountValue: number;
  minimumOrderAmount: number;
  expiryDate: string;
  isActive: boolean;
  usageLimit: number;
  usedCount: number;
}

export enum DiscountType {
  Percentage = 'Percentage',
  FixedAmount = 'FixedAmount'
}

// Shipping Address types
export interface ShippingAddress {
  id: number;
  userId: number;
  fullName: string;
  street: string;
  city: string;
  postalCode: string;
  country: string;
  phoneNumber: string;
  isDeleted: boolean;
}