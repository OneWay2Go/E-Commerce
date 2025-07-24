import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import { User, LoginRequest, RegisterRequest } from '@/types';
import { apiService } from '@/services/api';

interface AuthState {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;
  
  // Actions
  login: (credentials: LoginRequest) => Promise<void>;
  register: (userData: RegisterRequest) => Promise<void>;
  logout: () => void;
  clearError: () => void;
  setUser: (user: User) => void;
  handleUnauthorized: () => void;
  initializeAuth: () => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      user: null,
      isAuthenticated: false,
      isLoading: false,
      error: null,

      login: async (credentials: LoginRequest) => {
        set({ isLoading: true, error: null });
        
        try {
          const result = await apiService.login(credentials);
          
          if (result.succeeded) {
            const { accessToken, refreshToken, role } = result.data;
            
            // Store tokens in localStorage (already done in apiService)
            localStorage.setItem('refreshToken', refreshToken);
            
            // Create a mock user object based on the login response
            // In a real app, you'd decode the JWT or fetch user info
            const mockUser: User = {
              id: 1,
              fullName: credentials.email.split('@')[0], // Use email prefix as full name
              email: credentials.email,
            };
            
            set({
              user: mockUser,
              isAuthenticated: true,
              isLoading: false,
              error: null,
            });
          } else {
            set({
              isLoading: false,
              error: result.errors || 'Login failed',
            });
          }
        } catch (error: any) {
          set({
            isLoading: false,
            error: error.response?.data?.errors || error.message || 'Login failed',
          });
        }
      },

      register: async (userData: RegisterRequest) => {
        set({ isLoading: true, error: null });
        
        try {
          const result = await apiService.register(userData);
          
          if (result.succeeded) {
            set({
              isLoading: false,
              error: null,
            });
            // Registration successful, you might want to auto-login or redirect
          } else {
            set({
              isLoading: false,
              error: result.errors || 'Registration failed',
            });
          }
        } catch (error: any) {
          set({
            isLoading: false,
            error: error.response?.data?.errors || error.message || 'Registration failed',
          });
        }
      },

      logout: () => {
        apiService.clearAuthToken();
        
        set({
          user: null,
          isAuthenticated: false,
          error: null,
        });
      },

      // Method to handle 401 errors
      handleUnauthorized: () => {
        apiService.clearAuthToken();
        
        set({
          user: null,
          isAuthenticated: false,
          error: null,
        });
      },

      clearError: () => {
        set({ error: null });
      },

      setUser: (user: User) => {
        set({ user, isAuthenticated: true });
      },

      initializeAuth: () => {
        const token = localStorage.getItem('accessToken');
        if (token && !get().isAuthenticated) {
          // If we have a token but not authenticated, try to restore auth state
          set({ isAuthenticated: true });
        }
      },
    }),
    {
      name: 'auth-storage',
      storage: createJSONStorage(() => localStorage),
      partialize: (state) => ({
        user: state.user,
        isAuthenticated: state.isAuthenticated,
      }),
    }
  )
);