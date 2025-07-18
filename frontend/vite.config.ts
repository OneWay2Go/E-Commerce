import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
  server: {
    port: 3000,
    // Note: Since we're using direct API calls to the deployed backend,
    // proxy is not needed, but keeping it for local development fallback
    proxy: {
      '/api': {
        target: 'https://e-commerce-nr8a.onrender.com',
        changeOrigin: true,
        secure: true,
      }
    }
  }
})