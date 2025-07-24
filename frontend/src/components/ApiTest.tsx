import React, { useState } from 'react';
import { apiService } from '@/services/api';

const ApiTest: React.FC = () => {
  const [productsResult, setProductsResult] = useState<any>(null);
  const [categoriesResult, setCategoriesResult] = useState<any>(null);
  const [loading, setLoading] = useState(false);

  const testProducts = async () => {
    setLoading(true);
    try {
      const result = await apiService.getProducts();
      setProductsResult(result);
      console.log('Products API Result:', result);
    } catch (error) {
      setProductsResult({ error: error });
      console.error('Products API Error:', error);
    } finally {
      setLoading(false);
    }
  };

  const testCategories = async () => {
    setLoading(true);
    try {
      const result = await apiService.getCategories();
      setCategoriesResult(result);
      console.log('Categories API Result:', result);
    } catch (error) {
      setCategoriesResult({ error: error });
      console.error('Categories API Error:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <h2 className="text-2xl font-bold mb-6">API Test</h2>
      
      <div className="space-y-4">
        <div>
          <button 
            onClick={testProducts}
            disabled={loading}
            className="btn btn-primary btn-md mr-4"
          >
            {loading ? 'Testing...' : 'Test Products API'}
          </button>
          
          <button 
            onClick={testCategories}
            disabled={loading}
            className="btn btn-secondary btn-md"
          >
            {loading ? 'Testing...' : 'Test Categories API'}
          </button>
        </div>

        {productsResult && (
          <div className="bg-gray-100 p-4 rounded">
            <h3 className="font-bold mb-2">Products API Result:</h3>
            <pre className="text-sm overflow-auto">
              {JSON.stringify(productsResult, null, 2)}
            </pre>
          </div>
        )}

        {categoriesResult && (
          <div className="bg-gray-100 p-4 rounded">
            <h3 className="font-bold mb-2">Categories API Result:</h3>
            <pre className="text-sm overflow-auto">
              {JSON.stringify(categoriesResult, null, 2)}
            </pre>
          </div>
        )}
      </div>
    </div>
  );
};

export default ApiTest; 