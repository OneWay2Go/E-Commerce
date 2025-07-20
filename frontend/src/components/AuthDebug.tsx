import React, { useState } from 'react';
import { useAuthStore } from '@/store/authStore';
import { apiService } from '@/services/api';

const AuthDebug: React.FC = () => {
  const { isAuthenticated, user } = useAuthStore();
  const [testResult, setTestResult] = useState<string>('');
  const [loading, setLoading] = useState(false);

  const testProfile = async () => {
    setLoading(true);
    setTestResult('Testing profile API...');
    
    try {
      const response = await apiService.getUserProfile();
      setTestResult(`Profile API Response: ${JSON.stringify(response, null, 2)}`);
    } catch (error: any) {
      setTestResult(`Profile API Error: ${error.message} - Status: ${error.response?.status}`);
    } finally {
      setLoading(false);
    }
  };

  const testCart = async () => {
    setLoading(true);
    setTestResult('Testing cart API...');
    
    try {
      const response = await apiService.getMyCart();
      setTestResult(`Cart API Response: ${JSON.stringify(response, null, 2)}`);
    } catch (error: any) {
      setTestResult(`Cart API Error: ${error.message} - Status: ${error.response?.status}`);
    } finally {
      setLoading(false);
    }
  };

  const checkToken = () => {
    const token = localStorage.getItem('accessToken');
    setTestResult(`Token: ${token ? 'Present' : 'Missing'}\nToken Preview: ${token ? token.substring(0, 50) + '...' : 'N/A'}`);
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-2xl font-bold mb-4">Auth Debug</h1>
      
      <div className="bg-white rounded-lg shadow-sm border p-6 mb-6">
        <h2 className="text-lg font-semibold mb-4">Authentication Status</h2>
        <div className="space-y-2">
          <p><strong>Is Authenticated:</strong> {isAuthenticated ? 'Yes' : 'No'}</p>
          <p><strong>User:</strong> {user ? JSON.stringify(user) : 'None'}</p>
          <p><strong>Token in localStorage:</strong> {localStorage.getItem('accessToken') ? 'Yes' : 'No'}</p>
        </div>
      </div>

      <div className="bg-white rounded-lg shadow-sm border p-6 mb-6">
        <h2 className="text-lg font-semibold mb-4">API Tests</h2>
        <div className="space-y-4">
          <button
            onClick={checkToken}
            className="btn btn-outline btn-sm mr-2"
          >
            Check Token
          </button>
          <button
            onClick={testProfile}
            disabled={loading}
            className="btn btn-primary btn-sm mr-2"
          >
            Test Profile API
          </button>
          <button
            onClick={testCart}
            disabled={loading}
            className="btn btn-primary btn-sm"
          >
            Test Cart API
          </button>
        </div>
      </div>

      {testResult && (
        <div className="bg-white rounded-lg shadow-sm border p-6">
          <h2 className="text-lg font-semibold mb-4">Test Result</h2>
          <pre className="bg-gray-100 p-4 rounded text-sm overflow-auto">
            {testResult}
          </pre>
        </div>
      )}
    </div>
  );
};

export default AuthDebug; 