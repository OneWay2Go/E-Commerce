import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Header from '@/components/Header';
import Footer from '@/components/Footer';
import HomePage from '@/pages/HomePage';
import ProductsPage from '@/pages/ProductsPage';
import ProductDetailsPage from '@/pages/ProductDetailsPage';
import CategoriesPage from '@/pages/CategoriesPage';
import CartPage from '@/pages/CartPage';
import ProfilePage from '@/pages/ProfilePage';
import OrdersPage from '@/pages/OrdersPage';
import WishlistPage from '@/pages/WishlistPage';
import LoginForm from '@/components/LoginForm';
import RegisterForm from '@/components/RegisterForm';
import AuthDemo from '@/components/AuthDemo';
import ApiTest from '@/components/ApiTest';
import AuthDebug from '@/components/AuthDebug';

const NotFoundPage = () => (
  <div className="container mx-auto px-4 py-16 text-center">
    <h1 className="text-6xl font-bold text-gray-300 mb-4">404</h1>
    <h2 className="text-3xl font-bold text-gray-900 mb-4">Page Not Found</h2>
    <p className="text-gray-600 mb-8">
      The page you're looking for doesn't exist or has been moved.
    </p>
    <a href="/" className="btn btn-primary btn-md">
      Go Back Home
    </a>
  </div>
);

const App: React.FC = () => {
  return (
    <Router>
      <div className="min-h-screen flex flex-col bg-gray-50">
        <Header />
        
        <main className="flex-1">
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/products" element={<ProductsPage />} />
            <Route path="/products/:id" element={<ProductDetailsPage />} />
            <Route path="/categories" element={<CategoriesPage />} />
            <Route path="/cart" element={<CartPage />} />
            <Route path="/wishlist" element={<WishlistPage />} />
            <Route path="/profile" element={<ProfilePage />} />
            <Route path="/orders" element={<OrdersPage />} />
            <Route path="/login" element={<LoginForm />} />
            <Route path="/register" element={<RegisterForm />} />
            <Route path="/auth-demo" element={<AuthDemo />} />
            <Route path="/api-test" element={<ApiTest />} />
            <Route path="/auth-debug" element={<AuthDebug />} />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </main>
        
        <Footer />
      </div>
    </Router>
  );
};

export default App;