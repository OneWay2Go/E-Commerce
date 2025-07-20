import React from 'react';
import { useAuthStore } from '@/store/authStore';

const AuthDemo: React.FC = () => {
  const { isAuthenticated, user, logout } = useAuthStore();

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="max-w-2xl mx-auto">
        <h2 className="text-2xl font-bold mb-6">Authentication Demo</h2>
        
        <div className="bg-white p-6 rounded-lg shadow-md">
          <h3 className="text-lg font-semibold mb-4">Current Status</h3>
          
          <div className="space-y-4">
            <div className="flex items-center space-x-2">
              <span className="font-medium">Authentication Status:</span>
              <span className={`px-2 py-1 rounded text-sm ${
                isAuthenticated 
                  ? 'bg-green-100 text-green-800' 
                  : 'bg-red-100 text-red-800'
              }`}>
                {isAuthenticated ? 'Authenticated' : 'Not Authenticated'}
              </span>
            </div>
            
            {isAuthenticated && user && (
              <div className="bg-gray-50 p-4 rounded">
                <h4 className="font-medium mb-2">User Information:</h4>
                <div className="space-y-1 text-sm">
                  <div><span className="font-medium">Name:</span> {user.firstName} {user.lastName}</div>
                  <div><span className="font-medium">Email:</span> {user.email}</div>
                  <div><span className="font-medium">ID:</span> {user.id}</div>
                </div>
              </div>
            )}
            
            <div className="bg-gray-50 p-4 rounded">
              <h4 className="font-medium mb-2">Token Status:</h4>
              <div className="space-y-1 text-sm">
                <div>
                  <span className="font-medium">Access Token:</span> 
                  <span className={`ml-2 ${localStorage.getItem('accessToken') ? 'text-green-600' : 'text-red-600'}`}>
                    {localStorage.getItem('accessToken') ? 'Present' : 'Not Found'}
                  </span>
                </div>
                <div>
                  <span className="font-medium">Refresh Token:</span> 
                  <span className={`ml-2 ${localStorage.getItem('refreshToken') ? 'text-green-600' : 'text-red-600'}`}>
                    {localStorage.getItem('refreshToken') ? 'Present' : 'Not Found'}
                  </span>
                </div>
              </div>
            </div>
            
            {isAuthenticated && (
              <div className="pt-4">
                <button 
                  onClick={logout}
                  className="btn btn-outline btn-md"
                >
                  Logout
                </button>
              </div>
            )}
          </div>
        </div>
        
        <div className="mt-8 bg-blue-50 p-6 rounded-lg">
          <h3 className="text-lg font-semibold mb-4">How to Test</h3>
          <div className="space-y-2 text-sm">
            <p>1. <strong>Registration:</strong> Go to <a href="/register" className="text-blue-600 hover:underline">/register</a> to create a new account</p>
            <p>2. <strong>Login:</strong> Go to <a href="/login" className="text-blue-600 hover:underline">/login</a> to sign in with your credentials</p>
            <p>3. <strong>Protected Routes:</strong> After login, you can access protected features like cart and wishlist</p>
            <p>4. <strong>Token Management:</strong> Check the browser's developer tools → Application → Local Storage to see stored tokens</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AuthDemo; 