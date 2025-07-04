# ECommerce Furniture Application

A comprehensive full-stack ecommerce application for furniture retail, built according to the Technical Requirements Document (TRD) specifications.

## 🏗️ Architecture Overview

This application follows a multi-layered architecture pattern with clear separation of concerns:

- **Frontend**: React 18.x SPA with modern hooks and context API
- **Backend**: .NET 8 Web API with layered architecture
- **Database**: SQL Server with Entity Framework Core
- **Architecture**: Domain-Driven Design with Repository Pattern and Unit of Work

## 📁 Project Structure

```
ECommerceFurniture/
├── src/
│   ├── ECommerceFurniture.Domain/              # Domain Models
│   ├── ECommerceFurniture.Repository/          # Repository Layer
│   ├── ECommerceFurniture.DataAccess/          # Data Access Layer
│   ├── ECommerceFurniture.Business/            # Business Layer
│   ├── ECommerceFurniture.WebAPI/              # Web API Layer
│   └── ecommerce-furniture-react/              # React SPA
├── tests/
│   ├── ECommerceFurniture.Business.Tests/
│   ├── ECommerceFurniture.Repository.Tests/
│   └── ECommerceFurniture.WebAPI.Tests/
└── docs/
```

## 🚀 Features Implemented

### Backend (.NET 8 Web API)
- ✅ **Domain Layer**: Complete entity models (Product, Category, Cart, CartItem, ProductImage, ProductSpecification)
- ✅ **Repository Layer**: Generic repository pattern with specific implementations
- ✅ **Data Access Layer**: Entity Framework Core with DbContext and configurations
- ✅ **Business Layer**: Service interfaces and implementations with DTOs
- ✅ **API Layer**: RESTful controllers with full CRUD operations
- ✅ **Database**: Seed data with sample furniture products
- ✅ **Configuration**: CORS, Swagger/OpenAPI, dependency injection

### Frontend (React SPA)
- ✅ **Services**: API integration with axios
- ✅ **Context**: Cart state management with React Context API
- ✅ **Structure**: Component-based architecture with proper folder organization
- 🔄 **Components**: Core components (in progress)

## 🛠️ Technology Stack

### Backend
- .NET 8 (LTS)
- Entity Framework Core 9.x
- SQL Server LocalDB
- Swagger/OpenAPI
- xUnit for testing

### Frontend
- React 18.x
- Axios for API calls
- React Router DOM
- React Context API for state management
- Modern CSS with responsive design

## 📋 API Endpoints

### Products
- `GET /api/products` - Get all products with filtering
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/category/{categoryId}` - Get products by category
- `GET /api/products/search?term={searchTerm}` - Search products
- `GET /api/products/featured` - Get featured products

### Categories
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get category by ID
- `GET /api/categories/active` - Get active categories

### Cart
- `GET /api/cart/{sessionId}` - Get cart by session ID
- `POST /api/cart/add` - Add item to cart
- `PUT /api/cart/update` - Update cart item
- `DELETE /api/cart/remove/{cartItemId}` - Remove item from cart
- `DELETE /api/cart/clear/{sessionId}` - Clear cart

## 🏃‍♂️ Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+ and npm
- SQL Server LocalDB

### Backend Setup

1. **Navigate to the root directory**:
   ```bash
   cd EComDemoWithCursor
   ```

2. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

3. **Update database connection string** (if needed) in `src/ECommerceFurniture.WebAPI/appsettings.json`

4. **Run the API**:
   ```bash
   dotnet run --project src/ECommerceFurniture.WebAPI
   ```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `http://localhost:5000` (root URL)

### Frontend Setup

1. **Navigate to React app directory**:
   ```bash
   cd src/ecommerce-furniture-react
   ```

2. **Install dependencies**:
   ```bash
   npm install
   ```

3. **Create environment file** `.env`:
   ```
   REACT_APP_API_BASE_URL=http://localhost:5000/api
   ```

4. **Start React development server**:
   ```bash
   npm start
   ```

The React app will be available at `http://localhost:3000`

## 🗄️ Database Schema

The application automatically creates the database with the following tables:
- **Products**: Furniture items with specifications
- **Categories**: Hierarchical product categories
- **Carts**: Session-based shopping carts
- **CartItems**: Items in shopping carts
- **ProductImages**: Product image references
- **ProductSpecifications**: Product technical specifications

## 📦 Sample Data

The application includes seed data with sample furniture products:
- Modern 3-Seater Sofa
- Glass Coffee Table
- Executive Office Chair
- Queen Size Bed Frame

## 🧪 Testing

Run backend tests:
```bash
dotnet test
```

## 🔧 Development Notes

### Backend Architecture
- **Repository Pattern**: Abstraction layer for data access
- **Unit of Work**: Transaction management across repositories
- **DTOs**: Data transfer objects for API responses
- **Service Layer**: Business logic separation
- **Dependency Injection**: Clean separation of concerns

### Frontend Architecture
- **Component Structure**: Organized by feature (product, cart, category)
- **State Management**: React Context API for cart state
- **Service Layer**: Centralized API communication
- **Custom Hooks**: Reusable logic extraction

## 🚧 Remaining Development Tasks

To complete the frontend implementation:

1. **Create React Components**:
   - ProductList, ProductCard, ProductDetail
   - Cart, CartItem components
   - Category navigation
   - Header, Footer, Layout components

2. **Implement Pages**:
   - HomePage with featured products
   - ProductListPage with filtering
   - ProductDetailPage
   - CartPage

3. **Add Styling**:
   - Responsive CSS design
   - Modern UI components
   - Loading states and error handling

4. **Enhanced Features**:
   - Product search functionality
   - Category filtering
   - Pagination
   - Image galleries

## 🔐 Configuration

### Backend Configuration (`appsettings.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceFurnitureDB;Trusted_Connection=true"
  },
  "CorsOrigins": ["http://localhost:3000"]
}
```

### Frontend Configuration (`.env`)
```
REACT_APP_API_BASE_URL=http://localhost:5000/api
```

## 📚 API Documentation

Full API documentation is available via Swagger UI at the root URL when running the backend in development mode.

## 🤝 Contributing

This application serves as a proof of concept and follows enterprise-level architecture patterns suitable for production development.

## 📝 License

This project is created for demonstration purposes following the provided Technical Requirements Document.

---

**Built with ❤️ following enterprise-grade architecture patterns and best practices.** 