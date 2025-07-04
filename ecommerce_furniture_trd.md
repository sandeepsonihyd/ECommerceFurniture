# Technical Requirements Document (TRD)
## ECommerce Furniture App - Proof of Concept

---

### Document Information
- **Project Name:** ECommerce Furniture App
- **Document Type:** Technical Requirements Document
- **Version:** 1.0
- **Date:** July 1, 2025
- **Project Type:** Proof of Concept
- **Related Document:** Business Requirements Document v1.0

---

## 1. Technical Architecture Overview

### 1.1 System Architecture
The application follows a multi-layered architecture pattern with clear separation of concerns:

```
┌─────────────────────────────────────┐
│           React SPA Client                                          │
│        (Presentation Layer)                                      │
└─────────────────────────────────────┘
                    │ HTTP/HTTPS
                    ▼
┌─────────────────────────────────────┐
│         .NET 8 Web API              │
│      (API/Controller Layer)         │
└─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────┐
│         Business Layer              │
│    (Business Logic/Services)        │
└─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────┐
│       Data Access Layer             │
│    (Entity Framework Core)          │
└─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────┐
│      Repository Layer               │
│  (Generic Repository + UoW)         │
└─────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────┐
│         SQL Server Database         │
│        (Data Storage Layer)         │
└─────────────────────────────────────┘
```

### 1.2 Technology Stack
- **Backend Framework:** .NET 8 (LTS)
- **Database:** Microsoft SQL Server 2019/2022
- **ORM:** Entity Framework Core 8.x
- **Frontend Framework:** React 18.x
- **API Architecture:** RESTful Web API
- **Authentication:** JWT (for future implementation)
- **Logging:** Serilog
- **Testing:** xUnit, Moq

---

## 2. Project Structure and Layers

### 2.1 Solution Structure
```
ECommerceFurniture.sln
├── src/
│   ├── ECommerceFurniture.Domain/              # Domain Models
│   ├── ECommerceFurniture.Repository/         # Repository Layer
│   ├── ECommerceFurniture.DataAccess/        # Data Access Layer
│   ├── ECommerceFurniture.Business/           # Business Layer
│   ├── ECommerceFurniture.WebAPI/              # Web API Layer
│   └── ECommerceFurniture.React/               # React SPA
├── tests/
│   ├── ECommerceFurniture.Business.Tests/
│   ├── ECommerceFurniture.Repository.Tests/
│   └── ECommerceFurniture.WebAPI.Tests/
└── docs/
```

### 2.2 Layer-wise Technical Requirements

## 3. Domain Layer (ECommerceFurniture.Domain)

### 3.1 Entity Models
```csharp
// Core domain entities
- Product
- Category
- CartItem
- Cart
- ProductImage
- ProductSpecification
```

### 3.2 Domain Entity Specifications

#### 3.2.1 Product Entity
```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string SKU { get; set; }
    public int CategoryId { get; set; }
    public bool IsActive { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    
    // Navigation Properties
    public Category Category { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }
    public ICollection<ProductSpecification> Specifications { get; set; }
}
```

#### 3.2.2 Category Entity
```csharp
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int? ParentCategoryId { get; set; }
    
    // Navigation Properties
    public Category ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; }
    public ICollection<Product> Products { get; set; }
}
```

---

## 4. Repository Layer (ECommerceFurniture.Repository)

### 4.1 Generic Repository Pattern
```csharp
public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<bool> ExistsAsync(int id);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> expression);
}
```

### 4.2 Unit of Work Pattern
```csharp
public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    ICartRepository Carts { get; }
    ICartItemRepository CartItems { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
```

### 4.3 Specific Repository Interfaces
```csharp
public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    Task<IEnumerable<Product>> GetFeaturedProductsAsync();
    Task<Product> GetProductWithDetailsAsync(int id);
}

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IEnumerable<Category>> GetActiveCategoriesAsync();
    Task<IEnumerable<Category>> GetCategoriesWithProductCountAsync();
}
```

---

## 5. Data Access Layer (ECommerceFurniture.DataAccess)

### 5.1 DbContext Configuration
```csharp
public class ECommerceFurnitureDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ProductSpecification> ProductSpecifications { get; set; }
}
```

### 5.2 Entity Framework Configurations
- **Code First Approach** with Fluent API configurations
- **Migration Strategy** for database schema management
- **Connection String Management** through appsettings.json
- **Database Seeding** for initial product and category data

### 5.3 Database Schema Requirements

#### 5.3.1 Products Table
```sql
CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18,2) NOT NULL,
    SKU NVARCHAR(50) UNIQUE NOT NULL,
    CategoryId INT NOT NULL,
    IsActive BIT DEFAULT 1,
    StockQuantity INT DEFAULT 0,
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    ModifiedDate DATETIME2,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);
```

#### 5.3.2 Categories Table
```sql
CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(500),
    IsActive BIT DEFAULT 1,
    ParentCategoryId INT NULL,
    FOREIGN KEY (ParentCategoryId) REFERENCES Categories(Id)
);
```

---

## 6. Business Layer (ECommerceFurniture.Business)

### 6.1 Service Interfaces
```csharp
public interface IProductService
{
    Task<ProductDto> GetProductByIdAsync(int id);
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
    Task<PagedResult<ProductDto>> GetProductsPagedAsync(ProductFilterDto filter);
}

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto> GetCategoryByIdAsync(int id);
    Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync();
}

public interface ICartService
{
    Task<CartDto> GetCartAsync(string sessionId);
    Task<CartItemDto> AddToCartAsync(AddToCartDto addToCartDto);
    Task<CartItemDto> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto);
    Task<bool> RemoveFromCartAsync(int cartItemId);
    Task ClearCartAsync(string sessionId);
}
```

### 6.2 Data Transfer Objects (DTOs)
```csharp
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string SKU { get; set; }
    public string CategoryName { get; set; }
    public int StockQuantity { get; set; }
    public List<ProductImageDto> Images { get; set; }
}

public class CartDto
{
    public int Id { get; set; }
    public string SessionId { get; set; }
    public List<CartItemDto> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
}
```

### 6.3 Business Logic Requirements
- **Product Management:** CRUD operations with business validation
- **Cart Management:** Session-based cart handling without user authentication
- **Price Calculations:** Tax calculation, discount application logic
- **Inventory Management:** Stock level validation and management
- **Data Validation:** Input validation and business rule enforcement

---

## 7. Web API Layer (ECommerceFurniture.WebAPI)

### 7.1 API Controller Structure
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] ProductFilterDto filter)
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
    
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts([FromQuery] string term)
}
```

### 7.2 API Endpoints Specification

#### 7.2.1 Product Endpoints
- `GET /api/products` - Get all products with filtering
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/category/{categoryId}` - Get products by category
- `GET /api/products/search?term={searchTerm}` - Search products

#### 7.2.2 Category Endpoints
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get category by ID
- `GET /api/categories/active` - Get active categories

#### 7.2.3 Cart Endpoints
- `GET /api/cart/{sessionId}` - Get cart by session ID
- `POST /api/cart/add` - Add item to cart
- `PUT /api/cart/update` - Update cart item
- `DELETE /api/cart/remove/{cartItemId}` - Remove item from cart

### 7.3 API Configuration Requirements
- **CORS Configuration** for React frontend
- **JSON Serialization** settings
- **Error Handling** middleware
- **Logging** configuration
- **Swagger/OpenAPI** documentation
- **Rate Limiting** for API protection

---

## 8. React SPA Layer (ECommerceFurniture.React)

### 8.1 React Application Structure
```
src/
├── components/
│   ├── common/           # Reusable components
│   ├── product/          # Product-related components
│   ├── cart/             # Cart components
│   └── category/         # Category components
├── pages/
│   ├── HomePage.jsx
│   ├── ProductListPage.jsx
│   ├── ProductDetailPage.jsx
│   └── CartPage.jsx
├── services/
│   ├── api.js            # API service layer
│   ├── productService.js
│   ├── cartService.js
│   └── categoryService.js
├── hooks/
│   ├── useCart.js
│   ├── useProducts.js
│   └── useLocalStorage.js
├── context/
│   └── CartContext.jsx
├── utils/
└── styles/
```

### 8.2 Key React Components

#### 8.2.1 Product Components
```jsx
// ProductList.jsx
export const ProductList = ({ products, onAddToCart }) => {
    // Product grid display logic
};

// ProductCard.jsx
export const ProductCard = ({ product, onAddToCart }) => {
    // Individual product card component
};

// ProductDetail.jsx
export const ProductDetail = ({ productId }) => {
    // Detailed product view with specifications
};
```

#### 8.2.2 Cart Components
```jsx
// Cart.jsx
export const Cart = () => {
    // Shopping cart display and management
};

// CartItem.jsx
export const CartItem = ({ item, onUpdateQuantity, onRemove }) => {
    // Individual cart item component
};
```

### 8.3 State Management
- **React Context API** for cart state management
- **Custom Hooks** for API data fetching
- **Local Storage** for cart persistence
- **Session Management** for cart identification

### 8.4 Frontend Technical Requirements
- **Responsive Design** using CSS Grid/Flexbox
- **API Integration** with Axios or Fetch API
- **Error Handling** and loading states
- **Client-side Routing** with React Router
- **Performance Optimization** with React.memo and useMemo

---

## 9. Infrastructure and Deployment Requirements

### 9.1 Development Environment
- **IDE:** Visual Studio 2022 or Visual Studio Code
- **Node.js:** Version 18.x or higher
- **SQL Server:** LocalDB for development
- **Package Managers:** NuGet for .NET, npm/yarn for React

### 9.2 Database Requirements
- **SQL Server Version:** 2019 or later
- **Initial Database Size:** 100MB
- **Growth Settings:** Auto-grow by 10MB
- **Backup Strategy:** Daily backups for production
- **Indexing Strategy:** Primary keys, foreign keys, and search-optimized indexes

### 9.3 Performance Requirements
- **API Response Time:** < 500ms for standard queries
- **Database Query Performance:** < 100ms for simple queries
- **Frontend Load Time:** < 3 seconds initial load
- **Concurrent Users:** Support up to 100 concurrent users

---

## 10. Security Requirements

### 10.1 API Security
- **HTTPS Enforcement** for all API communications
- **Input Validation** on all API endpoints
- **SQL Injection Prevention** through parameterized queries
- **XSS Protection** headers implementation
- **Rate Limiting** to prevent abuse

### 10.2 Data Security
- **Connection String Encryption** in configuration
- **Sensitive Data Protection** in logs
- **Error Message Sanitization** to prevent information disclosure

---

## 11. Testing Requirements

### 11.1 Unit Testing
- **Repository Layer:** Test repository implementations
- **Business Layer:** Test service logic and validations
- **API Layer:** Test controller actions and responses
- **Code Coverage:** Minimum 80% coverage target

### 11.2 Integration Testing
- **Database Integration:** Test Entity Framework configurations
- **API Integration:** Test end-to-end API workflows
- **Frontend Integration:** Test React component interactions

---

## 12. Configuration and Environment Settings

### 12.1 Configuration Files
```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceFurnitureDB;Trusted_Connection=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*",
  "CorsOrigins": ["http://localhost:3000"]
}
```

### 12.2 Environment Variables
- **Database Connection Strings**
- **API Base URLs**
- **Logging Levels**
- **CORS Settings**

---

## 13. Monitoring and Logging

### 13.1 Logging Requirements
- **Structured Logging** with Serilog
- **Log Levels:** Debug, Information, Warning, Error, Critical
- **Log Destinations:** File, Console, Database (optional)
- **Performance Logging** for slow queries and operations

### 13.2 Monitoring
- **Application Health Checks**
- **Database Performance Monitoring**
- **API Response Time Tracking**
- **Error Rate Monitoring**

---

## 14. Deployment Architecture

### 14.1 Deployment Strategy
- **Backend Deployment:** IIS or Azure App Service
- **Frontend Deployment:** Static hosting (Azure Static Web Apps, Netlify)
- **Database Deployment:** SQL Server on-premises or Azure SQL Database
- **CI/CD Pipeline:** Azure DevOps or GitHub Actions

### 14.2 Environment Configuration
- **Development:** Local development with LocalDB
- **Staging:** Cloud-based testing environment
- **Production:** Scalable cloud deployment

---

## 15. Technical Deliverables

### 15.1 Code Deliverables
- Complete .NET 8 solution with all layers implemented
- React SPA application with responsive design
- Database schema with initial seed data
- Unit and integration test suites
- API documentation (Swagger)

### 15.2 Documentation Deliverables
- Database schema documentation
- API endpoint documentation
- Deployment guide
- Developer setup instructions
- Architecture decision records (ADRs)

---

## 16. Success Criteria

### 16.1 Technical Success Metrics
- All API endpoints return expected responses
- Frontend successfully consumes all API endpoints
- Cart functionality persists across browser sessions
- Database operations perform within specified time limits
- Application deploys successfully in target environment

### 16.2 Quality Metrics
- Code coverage exceeds 80%
- No critical security vulnerabilities
- All performance benchmarks met
- Cross-browser compatibility achieved
- Mobile responsiveness validated

---

**Document Approval:**
- Technical Architect: [To be assigned]
- Senior Developer: [To be assigned]
- Database Administrator: [To be assigned]
- DevOps Engineer: [To be assigned]