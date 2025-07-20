import React, { useState, useEffect } from 'react';
import { useAuthStore } from '@/store/authStore';
import { apiService } from '@/services/api';
import { Order, OrderStatus } from '@/types';
import { Package, Calendar, DollarSign, Eye, Truck, CheckCircle, XCircle, Clock } from 'lucide-react';

const OrdersPage: React.FC = () => {
  const { isAuthenticated } = useAuthStore();
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (isAuthenticated) {
      loadOrders();
    } else {
      setLoading(false);
    }
  }, [isAuthenticated]);

  const loadOrders = async () => {
    try {
      setLoading(true);
      const response = await apiService.getMyOrders();
      if (response.succeeded && response.data) {
        setOrders(response.data);
      } else {
        setError(response.errors || 'Failed to load orders');
      }
    } catch (err) {
      setError('Failed to load orders');
      console.error('Error loading orders:', err);
    } finally {
      setLoading(false);
    }
  };

  const getStatusIcon = (status: OrderStatus) => {
    switch (status) {
      case OrderStatus.Pending:
        return <Clock className="h-5 w-5 text-yellow-500" />;
      case OrderStatus.Processing:
        return <Package className="h-5 w-5 text-blue-500" />;
      case OrderStatus.Shipped:
        return <Truck className="h-5 w-5 text-purple-500" />;
      case OrderStatus.Delivered:
        return <CheckCircle className="h-5 w-5 text-green-500" />;
      case OrderStatus.Cancelled:
        return <XCircle className="h-5 w-5 text-red-500" />;
      default:
        return <Package className="h-5 w-5 text-gray-500" />;
    }
  };

  const getStatusColor = (status: OrderStatus) => {
    switch (status) {
      case OrderStatus.Pending:
        return 'bg-yellow-100 text-yellow-800';
      case OrderStatus.Processing:
        return 'bg-blue-100 text-blue-800';
      case OrderStatus.Shipped:
        return 'bg-purple-100 text-purple-800';
      case OrderStatus.Delivered:
        return 'bg-green-100 text-green-800';
      case OrderStatus.Cancelled:
        return 'bg-red-100 text-red-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  if (!isAuthenticated) {
    return (
      <div className="container mx-auto px-4 py-16 text-center">
        <Package className="h-16 w-16 text-gray-400 mx-auto mb-4" />
        <h1 className="text-2xl font-bold text-gray-900 mb-4">Sign in to view your orders</h1>
        <p className="text-gray-600 mb-8">
          You need to be signed in to view your order history.
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
            onClick={loadOrders}
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
        <h1 className="text-3xl font-bold text-gray-900">My Orders</h1>
        <button
          onClick={loadOrders}
          className="btn btn-outline btn-sm"
        >
          Refresh
        </button>
      </div>

      {orders.length === 0 ? (
        <div className="text-center py-16">
          <Package className="h-16 w-16 text-gray-400 mx-auto mb-4" />
          <h2 className="text-xl font-semibold text-gray-900 mb-2">No orders yet</h2>
          <p className="text-gray-600 mb-8">
            You haven't placed any orders yet. Start shopping to see your order history here.
          </p>
          <a
            href="/products"
            className="btn btn-primary btn-lg"
          >
            Start Shopping
          </a>
        </div>
      ) : (
        <div className="space-y-6">
          {orders.map((order) => (
            <div key={order.id} className="bg-white rounded-lg shadow-sm border">
              {/* Order Header */}
              <div className="p-6 border-b border-gray-200">
                <div className="flex items-center justify-between">
                  <div className="flex items-center space-x-4">
                    <div className="flex items-center space-x-2">
                      {getStatusIcon(order.status)}
                      <span className={`px-3 py-1 rounded-full text-sm font-medium ${getStatusColor(order.status)}`}>
                        {order.status}
                      </span>
                    </div>
                    <div className="text-sm text-gray-500">
                      Order #{order.id}
                    </div>
                  </div>
                  <div className="text-right">
                    <div className="text-lg font-bold text-gray-900">
                      ${order.totalAmount.toFixed(2)}
                    </div>
                    <div className="text-sm text-gray-500">
                      {formatDate(order.orderDate)}
                    </div>
                  </div>
                </div>
              </div>

              {/* Order Details */}
              <div className="p-6">
                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                  {/* Order Info */}
                  <div>
                    <h3 className="font-semibold text-gray-900 mb-2">Order Information</h3>
                    <div className="space-y-2 text-sm">
                      <div className="flex items-center space-x-2">
                        <Calendar className="h-4 w-4 text-gray-400" />
                        <span className="text-gray-600">
                          Placed on {formatDate(order.orderDate)}
                        </span>
                      </div>
                      <div className="flex items-center space-x-2">
                        <DollarSign className="h-4 w-4 text-gray-400" />
                        <span className="text-gray-600">
                          Total: ${order.totalAmount.toFixed(2)}
                        </span>
                      </div>
                      {order.notes && (
                        <div className="text-gray-600">
                          <span className="font-medium">Notes:</span> {order.notes}
                        </div>
                      )}
                    </div>
                  </div>

                  {/* Shipping Address */}
                  <div>
                    <h3 className="font-semibold text-gray-900 mb-2">Shipping Address</h3>
                    <div className="text-sm text-gray-600">
                      <p>Address ID: {order.shippingAddressId}</p>
                      <p className="text-gray-500">Address details will be shown here</p>
                    </div>
                  </div>

                  {/* Actions */}
                  <div>
                    <h3 className="font-semibold text-gray-900 mb-2">Actions</h3>
                    <div className="space-y-2">
                      <button
                        className="w-full btn btn-outline btn-sm flex items-center justify-center space-x-2"
                        onClick={() => {
                          // TODO: Implement order details view
                          alert(`Viewing order details for order #${order.id}`);
                        }}
                      >
                        <Eye className="h-4 w-4" />
                        <span>View Details</span>
                      </button>
                      {order.status === OrderStatus.Delivered && (
                        <button
                          className="w-full btn btn-outline btn-sm"
                          onClick={() => {
                            // TODO: Implement reorder functionality
                            alert(`Reordering items from order #${order.id}`);
                          }}
                        >
                          Reorder
                        </button>
                      )}
                      {order.status === OrderStatus.Shipped && (
                        <button
                          className="w-full btn btn-outline btn-sm"
                          onClick={() => {
                            // TODO: Implement tracking functionality
                            alert(`Tracking order #${order.id}`);
                          }}
                        >
                          Track Package
                        </button>
                      )}
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Order Status Legend */}
      <div className="mt-8 bg-white rounded-lg shadow-sm border p-6">
        <h3 className="font-semibold text-gray-900 mb-4">Order Status Guide</h3>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4">
          <div className="flex items-center space-x-2">
            <Clock className="h-4 w-4 text-yellow-500" />
            <span className="text-sm text-gray-600">Pending</span>
          </div>
          <div className="flex items-center space-x-2">
            <Package className="h-4 w-4 text-blue-500" />
            <span className="text-sm text-gray-600">Processing</span>
          </div>
          <div className="flex items-center space-x-2">
            <Truck className="h-4 w-4 text-purple-500" />
            <span className="text-sm text-gray-600">Shipped</span>
          </div>
          <div className="flex items-center space-x-2">
            <CheckCircle className="h-4 w-4 text-green-500" />
            <span className="text-sm text-gray-600">Delivered</span>
          </div>
          <div className="flex items-center space-x-2">
            <XCircle className="h-4 w-4 text-red-500" />
            <span className="text-sm text-gray-600">Cancelled</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default OrdersPage; 