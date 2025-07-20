# Authentication Implementation

This document describes the authentication system implemented in the E-Commerce frontend application.

## Overview

The authentication system provides user registration and login functionality with JWT token management, form validation, and proper error handling.

## Features

### ✅ Implemented Features

1. **User Registration**
   - Form validation (email, password, full name, phone number)
   - Password confirmation
   - Real-time validation feedback
   - Error handling and display

2. **User Login**
   - Email and password authentication
   - Form validation
   - JWT token storage
   - Automatic redirect after successful login

3. **Token Management**
   - Automatic token storage in localStorage
   - Token inclusion in API request headers
   - Token cleanup on logout
   - 401 error handling with automatic logout

4. **UI/UX Features**
   - Loading states during authentication
   - Error message display
   - Form validation with visual feedback
   - Responsive design
   - Navigation between login/register pages

## API Integration

### Backend Endpoints

The frontend integrates with the following backend endpoints:

#### Registration
- **URL**: `POST /auth/register`
- **Body**: 
  ```json
  {
    "fullName": "string",
    "email": "string", 
    "password": "string",
    "phoneNumber": "string"
  }
  ```
- **Response**: 
  ```json
  {
    "data": {
      "email": "string",
      "message": "string"
    },
    "succeeded": boolean,
    "errors": "string"
  }
  ```

#### Login
- **URL**: `POST /auth/login`
- **Body**:
  ```json
  {
    "email": "string",
    "password": "string"
  }
  ```
- **Response**:
  ```json
  {
    "data": {
      "accessToken": "string",
      "refreshToken": "string", 
      "role": "string"
    },
    "succeeded": boolean,
    "errors": "string"
  }
  ```

### Token Management

After successful login:
1. `accessToken` is automatically stored in localStorage
2. `accessToken` is added to all subsequent API request headers as `Authorization: Bearer {token}`
3. `refreshToken` is stored for future refresh functionality
4. On logout or 401 errors, tokens are automatically cleared

## Components

### LoginForm Component
- **Location**: `src/components/LoginForm.tsx`
- **Features**:
  - Email and password validation
  - Loading states
  - Error display
  - Automatic redirect after login
  - Link to registration page

### RegisterForm Component  
- **Location**: `src/components/RegisterForm.tsx`
- **Features**:
  - Full form validation (name, email, phone, password, confirm password)
  - Password confirmation matching
  - Loading states
  - Error display
  - Success redirect to login page
  - Link to login page

## State Management

### Auth Store (Zustand)
- **Location**: `src/store/authStore.ts`
- **Features**:
  - User authentication state
  - Loading states
  - Error handling
  - Token management
  - Persistent storage

### API Service
- **Location**: `src/services/api.ts`
- **Features**:
  - Centralized API calls
  - Automatic token injection
  - Error handling
  - Token management methods

## Form Validation

### Login Form Validation
- Email: Required, valid email format
- Password: Required, minimum 6 characters

### Registration Form Validation
- Full Name: Required, non-empty
- Email: Required, valid email format
- Phone Number: Required, non-empty
- Password: Required, minimum 6 characters
- Confirm Password: Required, must match password

## Error Handling

### API Errors
- Network errors are caught and displayed
- Backend validation errors are shown to user
- 401 errors trigger automatic logout

### Form Validation Errors
- Real-time validation feedback
- Visual indicators (red borders)
- Error messages below each field
- Errors clear when user starts typing

## Security Features

1. **Token Storage**: JWT tokens stored in localStorage
2. **Automatic Token Injection**: All API requests include Authorization header
3. **Token Cleanup**: Tokens removed on logout or 401 errors
4. **Form Validation**: Client-side validation prevents invalid submissions
5. **Error Handling**: Sensitive error information is not exposed

## Usage Examples

### Login Flow
```typescript
import { useAuthStore } from '@/store/authStore';

const { login, isLoading, error } = useAuthStore();

const handleLogin = async (credentials) => {
  await login(credentials);
  // User will be automatically redirected on success
};
```

### Registration Flow
```typescript
import { useAuthStore } from '@/store/authStore';

const { register, isLoading, error } = useAuthStore();

const handleRegister = async (userData) => {
  await register(userData);
  // User will be redirected to login page on success
};
```

### Check Authentication Status
```typescript
import { useAuthStore } from '@/store/authStore';

const { isAuthenticated, user } = useAuthStore();

if (isAuthenticated) {
  console.log('User is logged in:', user);
}
```

## Testing

A test file is included at `src/test-auth.ts` with functions to verify:
- Token management
- Registration functionality  
- Login functionality

To run tests, import and call the test functions in the browser console.

## Future Enhancements

1. **Refresh Token Implementation**: Automatic token refresh on expiration
2. **Remember Me**: Persistent login option
3. **Password Reset**: Forgot password functionality
4. **Email Verification**: Email confirmation flow
5. **Social Login**: OAuth integration (Google, Facebook, etc.)
6. **Two-Factor Authentication**: Additional security layer

## Troubleshooting

### Common Issues

1. **CORS Errors**: Ensure backend allows frontend domain
2. **Token Not Stored**: Check localStorage permissions
3. **Form Not Submitting**: Verify all required fields are filled
4. **API Errors**: Check network tab for detailed error information

### Debug Mode

Enable debug logging by adding to browser console:
```javascript
localStorage.setItem('debug', 'auth:*');
```

## Dependencies

- **React**: UI framework
- **React Router**: Navigation
- **Zustand**: State management
- **Axios**: HTTP client
- **Tailwind CSS**: Styling
- **Lucide React**: Icons 