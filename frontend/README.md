# E-Commerce Frontend

A modern, responsive e-commerce frontend built with React, TypeScript, and Tailwind CSS. This application provides a complete shopping experience with user authentication, product browsing, cart management, and order processing.

## 🚀 Features

- **Modern UI/UX**: Clean, responsive design with smooth animations
- **Authentication**: User registration, login, and session management
- **Product Management**: Browse products, view details, search and filter
- **Shopping Cart**: Add/remove items, quantity management, persistent cart
- **Categories**: Organized product categories with navigation
- **Wishlist**: Save favorite products for later
- **User Dashboard**: Profile management, order history, settings
- **Responsive Design**: Optimized for desktop, tablet, and mobile devices
- **Performance**: Lazy loading, code splitting, and optimizations

## 🛠️ Technology Stack

- **React 18** - UI library with modern hooks and concurrent features
- **TypeScript** - Type-safe development
- **Vite** - Fast build tool and development server
- **Tailwind CSS** - Utility-first CSS framework
- **React Router** - Client-side routing
- **Zustand** - Lightweight state management
- **Axios** - HTTP client for API communication
- **Lucide React** - Beautiful icon library

## 📦 Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd frontend
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Environment Setup**
   Create a `.env` file in the root directory:
   ```env
   VITE_API_BASE_URL=http://localhost:5000/api
   VITE_APP_NAME=E-Commerce Store
   ```

4. **Start the development server**
   ```bash
   npm run dev
   ```

5. **Build for production**
   ```bash
   npm run build
   ```

## 🏗️ Project Structure

```
src/
├── components/          # Reusable UI components
│   ├── Header.tsx       # Navigation and user menu
│   ├── Footer.tsx       # Footer with links and info
│   └── ProductCard.tsx  # Product display component
├── pages/              # Page components
│   └── HomePage.tsx    # Main landing page
├── store/              # State management
│   ├── authStore.ts    # Authentication state
│   └── cartStore.ts    # Shopping cart state
├── services/           # API services
│   └── api.ts          # HTTP client and endpoints
├── types/              # TypeScript type definitions
│   └── index.ts        # Shared types and interfaces
├── lib/                # Utility functions
│   └── utils.ts        # Helper functions
├── App.tsx             # Main app component with routing
├── main.tsx            # Application entry point
└── index.css           # Global styles and Tailwind imports
```

## 🎨 Styling

The project uses Tailwind CSS for styling with:

- **Custom color palette** for brand consistency
- **Component classes** for reusable UI elements
- **Responsive design** with mobile-first approach
- **Dark mode support** (configurable)
- **Smooth animations** and transitions

## 🔧 API Integration

The frontend integrates with a .NET Core backend API that provides:

- **Authentication endpoints** (`/auth/login`, `/auth/register`)
- **Product management** (`/api/product`)
- **Category management** (`/api/category`)
- **Shopping cart** (`/api/cartitem`)
- **Order processing** (`/api/order`)
- **User management** (`/api/user`)

## 🚦 Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint

## 🔒 Authentication

The application supports:

- **JWT-based authentication** with access and refresh tokens
- **Protected routes** that require authentication
- **Persistent sessions** using localStorage
- **Automatic token refresh** and logout on expiration

## 🛒 Shopping Cart

Features include:

- **Add to cart** from product listings
- **Quantity management** with increment/decrement
- **Persistent cart** across browser sessions
- **Real-time total** calculation
- **Cart badge** in navigation

## 📱 Responsive Design

The application is fully responsive with:

- **Mobile-first** design approach
- **Breakpoint-specific** layouts and components
- **Touch-friendly** interactions
- **Optimized images** and content

## 🌟 Performance Optimizations

- **Code splitting** with React.lazy()
- **Image optimization** with placeholder images
- **Lazy loading** for product images
- **Memoization** for expensive computations
- **Bundle optimization** with Vite

## 🧪 Development Guidelines

### Code Style
- Use TypeScript for all new files
- Follow React best practices and hooks patterns
- Use Tailwind CSS utility classes
- Implement proper error handling

### State Management
- Use Zustand for global state
- Keep component state local when possible
- Use proper TypeScript typing for all stores

### API Calls
- Use the centralized API service
- Implement proper error handling
- Show loading states for async operations

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:

- Create an issue in the repository
- Contact the development team
- Check the documentation

## 🗺️ Roadmap

Upcoming features:

- [ ] Product reviews and ratings
- [ ] Advanced search and filtering
- [ ] Wishlist functionality
- [ ] Order tracking
- [ ] Email notifications
- [ ] Social media integration
- [ ] Progressive Web App (PWA) features
- [ ] Multi-language support
- [ ] Dark mode toggle

---

Built with ❤️ using React and TypeScript