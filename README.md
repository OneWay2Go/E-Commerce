# E-Commerce Platform

A modern, full-stack e-commerce platform built with .NET 8 and React. This comprehensive solution provides a scalable backend API and a responsive frontend for managing online retail operations.

## 🚀 Features

### Backend Features
- **Clean Architecture** with Domain-Driven Design (DDD) principles
- **RESTful API** with comprehensive endpoints
- **JWT Authentication** with role-based authorization
- **Entity Framework Core** with PostgreSQL database
- **Repository Pattern** with generic implementations
- **Permission-based Access Control** with seeded permissions
- **Email Services** for notifications
- **Logging** with Serilog
- **API Documentation** with Swagger/OpenAPI
- **Docker Support** for containerized deployment

### Frontend Features
- **Modern React 18** with TypeScript
- **Responsive Design** with Tailwind CSS
- **State Management** with Zustand
- **Authentication** with JWT token management
- **Shopping Cart** with persistent storage
- **Product Catalog** with search and filtering
- **User Dashboard** with order management
- **Wishlist** functionality
- **Category Navigation**
- **Mobile-First** responsive design

### Business Features
- **Product Management** - Create, update, and organize products
- **Category Management** - Hierarchical product categorization
- **Shopping Cart** - Add, remove, and manage cart items
- **Order Processing** - Complete order management system
- **Payment Integration** - Payment processing support
- **User Management** - Registration, authentication, and profiles
- **Review System** - Product reviews and ratings
- **Coupon System** - Discount codes and promotions
- **Wishlist** - Save products for later
- **Shipping Addresses** - Multiple address management

## 🏗️ Architecture

### Backend Architecture (Clean Architecture)
```
src/
├── ECommerce.API/           # Presentation Layer
│   ├── Controllers/         # API Controllers
│   ├── Extensions/          # Service registrations
│   └── Program.cs           # Application entry point
├── ECommerce.Application/   # Application Layer
│   ├── Interfaces/          # Repository interfaces
│   ├── Models/              # DTOs and ViewModels
│   └── Mappers/             # Object mapping
├── ECommerce.Domain/        # Domain Layer
│   ├── Entities/            # Domain entities
│   └── Enums/               # Domain enumerations
└── ECommerce.Infrastructure/ # Infrastructure Layer
    ├── Auth/                # Authentication services
    ├── Persistence/         # Database context and repositories
    ├── Services/            # External services
    └── Migrations/          # EF Core migrations
```

### Frontend Architecture
```
frontend/
├── src/
│   ├── components/          # Reusable UI components
│   ├── pages/              # Page components
│   ├── store/              # State management (Zustand)
│   ├── services/           # API communication
│   ├── types/              # TypeScript definitions
│   └── lib/                # Utility functions
├── public/                 # Static assets
└── dist/                   # Production build
```

## 🛠️ Technology Stack

### Backend
- **.NET 8** - Modern web framework
- **Entity Framework Core** - ORM with PostgreSQL
- **JWT Authentication** - Secure token-based auth
- **Serilog** - Structured logging
- **Swagger** - API documentation
- **Docker** - Containerization

### Frontend
- **React 18** - Modern UI library
- **TypeScript** - Type-safe development
- **Vite** - Fast build tool
- **Tailwind CSS** - Utility-first styling
- **Zustand** - Lightweight state management
- **React Router** - Client-side routing
- **Axios** - HTTP client
- **Lucide React** - Icon library

### Database
- **PostgreSQL** - Primary database
- **Entity Framework Core** - Database access
- **Repository Pattern** - Data access abstraction

## 📦 Installation & Setup

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [PostgreSQL](https://www.postgresql.org/) or Docker
- [Git](https://git-scm.com/)

### Backend Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd ecommerce-platform
   ```

2. **Database Setup**
   ```bash
   # Using Docker (recommended)
   docker run --name ecommerce-db -e POSTGRES_PASSWORD=your_password -e POSTGRES_DB=ecommercedb -p 5432:5432 -d postgres:latest
   
   # Or install PostgreSQL locally and create database
   createdb ecommercedb
   ```

3. **Configure Connection String**
   Update `src/ECommerce.API/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=ecommercedb;Username=postgres;Password=your_password"
     }
   }
   ```

4. **Run Database Migrations**
   ```bash
   cd src/ECommerce.API
   dotnet ef database update
   ```

5. **Start the API**
   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:7000`

### Frontend Setup

1. **Navigate to frontend directory**
   ```bash
   cd frontend
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Configure Environment**
   Create `.env` file:
   ```env
   VITE_API_BASE_URL=https://localhost:7000/api
   VITE_APP_NAME=E-Commerce Store
   ```

4. **Start the development server**
   ```bash
   npm run dev
   ```

   The frontend will be available at `http://localhost:3000`

### Docker Deployment

1. **Build and run with Docker Compose** (if available)
   ```bash
   docker-compose up -d
   ```

2. **Or build API container manually**
   ```bash
   cd src/ECommerce.API
   docker build -t ecommerce-api .
   docker run -p 8080:8080 ecommerce-api
   ```

## 🔧 Configuration

### Backend Configuration
Key configuration sections in `appsettings.json`:
- **ConnectionStrings** - Database connections
- **JwtOptions** - JWT token settings
- **EmailSettings** - Email service configuration
- **Serilog** - Logging configuration

### Frontend Configuration
Environment variables in `.env`:
- **VITE_API_BASE_URL** - Backend API URL
- **VITE_APP_NAME** - Application name

## 📚 API Documentation

When running the backend in development mode, visit:
- **Swagger UI**: `https://localhost:7000/swagger`
- **API Endpoints**: `https://localhost:7000/api`

### Main API Endpoints
- **Authentication**: `/api/auth/*`
- **Products**: `/api/product/*`
- **Categories**: `/api/category/*`
- **Cart**: `/api/cart/*`
- **Orders**: `/api/order/*`
- **Users**: `/api/user/*`
- **Reviews**: `/api/review/*`

## 🧪 Testing

### Backend Testing
```bash
cd src/ECommerce.API
dotnet test
```

### Frontend Testing
```bash
cd frontend
npm run test
```

## 🚦 Available Scripts

### Backend
- `dotnet run` - Start development server
- `dotnet build` - Build the project
- `dotnet test` - Run tests
- `dotnet ef migrations add <name>` - Create migration
- `dotnet ef database update` - Apply migrations

### Frontend
- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint

## 🔒 Authentication & Authorization

### Features
- **JWT Token-based Authentication**
- **Role-based Authorization**
- **Permission-based Access Control**
- **User Registration and Login**
- **Secure Password Handling**
- **Token Refresh Mechanism**

### Default Roles & Permissions
The system automatically seeds permissions and roles on startup.

## 🗄️ Database Schema

### Core Entities
- **User** - User accounts and authentication
- **Product** - Product catalog
- **Category** - Product categorization
- **Cart/CartItem** - Shopping cart management
- **Order/OrderItem** - Order processing
- **Payment** - Payment information
- **Review** - Product reviews
- **WishList** - User wishlists
- **Coupon** - Discount system
- **ShippingAddress** - Address management

## 🌐 Deployment

### Production Deployment
1. **Backend**: Deploy to cloud providers (Azure, AWS, etc.) or use Docker containers
2. **Frontend**: Deploy to static hosting (Vercel, Netlify, etc.) or serve from backend
3. **Database**: Use managed PostgreSQL service

### Environment Variables
Ensure proper environment variables are set for production:
- Database connection strings
- JWT secrets
- Email service credentials
- CORS origins

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines
- Follow Clean Architecture principles
- Use proper TypeScript types
- Write meaningful commit messages
- Add tests for new features
- Update documentation

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- **Uchqunov Muhammadamin** - *Initial work*

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Check the API documentation

## 🗺️ Roadmap

### Upcoming Features
- [ ] Advanced search and filtering
- [ ] Real-time notifications
- [ ] Multi-language support
- [ ] Progressive Web App (PWA)
- [ ] Analytics dashboard
- [ ] Social media integration
- [ ] Advanced reporting
- [ ] Multi-vendor support
- [ ] Mobile app development
- [ ] AI-powered recommendations

### Technical Improvements
- [ ] Comprehensive test coverage
- [ ] Performance optimizations
- [ ] Caching implementation
- [ ] Message queuing
- [ ] Microservices architecture
- [ ] GraphQL API option

## 📊 Project Status

- ✅ **Backend API** - Fully functional
- ✅ **Frontend App** - Production ready
- ✅ **Authentication** - Implemented
- ✅ **Database** - PostgreSQL with EF Core
- ✅ **Docker Support** - Available
- 🚧 **Testing** - In progress
- 🚧 **Documentation** - Expanding

---

Built with ❤️ using .NET 8 and React 18
