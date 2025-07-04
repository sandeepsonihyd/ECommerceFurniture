# ECommerce Furniture React Frontend

A modern, responsive React frontend for the ECommerce Furniture application built with React 19, Tailwind CSS, and integrating with the .NET Web API backend.

## 🚀 Features

### Core Functionality
- **Product Catalog**: Browse, search, and filter furniture products
- **Product Details**: Detailed product views with image galleries and specifications
- **Shopping Cart**: Add, update, and remove items from cart with real-time updates
- **Categories**: Browse products by furniture categories
- **Responsive Design**: Mobile-first design that works on all devices

### Technical Features
- **Modern React 19**: Latest React features and hooks
- **Tailwind CSS**: Utility-first CSS framework for beautiful styling
- **React Router**: Client-side routing for SPA experience
- **Context API**: Global state management for cart functionality
- **Axios Integration**: HTTP client for API communication
- **Hot Toast Notifications**: User feedback for actions
- **Component-Based Architecture**: Reusable, modular components

## 🏗️ Project Structure

```
src/
├── components/
│   ├── common/           # Reusable UI components
│   │   ├── Header.jsx    # Navigation header with cart
│   │   ├── Footer.jsx    # Site footer
│   │   ├── Button.jsx    # Reusable button component
│   │   └── Loading.jsx   # Loading spinner
│   ├── product/          # Product-related components
│   │   ├── ProductCard.jsx     # Product grid item
│   │   ├── ProductList.jsx     # Product listing with filters
│   │   └── ProductDetails.jsx  # Detailed product view
│   ├── category/         # Category components
│   │   ├── CategoryCard.jsx    # Category display card
│   │   └── CategoryList.jsx    # Category listing
│   └── cart/             # Shopping cart components
│       ├── CartItem.jsx        # Individual cart item
│       └── CartSummary.jsx     # Cart totals and checkout
├── pages/                # Page components
│   ├── HomePage.jsx      # Landing page with featured content
│   ├── ProductsPage.jsx  # Products listing page
│   ├── ProductDetailPage.jsx  # Individual product page
│   ├── CategoriesPage.jsx     # Categories browsing page
│   └── CartPage.jsx      # Shopping cart page
├── context/              # React Context providers
│   └── CartContext.jsx   # Global cart state management
├── services/             # API integration
│   └── api.js           # API client and service methods
├── styles/               # Additional CSS if needed
└── utils/                # Utility functions
```

## 🛠️ Technologies Used

- **React 19.1.0** - Frontend library
- **React Router DOM 7.6.3** - Client-side routing
- **Tailwind CSS 3.3.2** - Utility-first CSS framework
- **Axios 1.10.0** - HTTP client for API requests
- **Lucide React 0.263.1** - Modern icon library
- **React Hot Toast 2.4.1** - Toast notifications
- **PostCSS & Autoprefixer** - CSS processing

## 🎨 Design System

### Colors
- **Primary**: Blue-gray tones for professional appearance
- **Secondary**: Warm orange for accents and call-to-action
- **Typography**: Inter font family for modern readability

### Components
- Consistent button styles with multiple variants
- Card-based layouts for products and categories
- Responsive grid systems
- Smooth animations and transitions

## 🔌 API Integration

The frontend integrates with the .NET Web API backend running on `http://localhost:5009/api`:

### Endpoints Used:
- **Products**: `/api/products` - Get all products, search, featured products
- **Categories**: `/api/categories` - Get all categories and active categories
- **Cart**: `/api/cart` - Full cart management (add, update, remove, clear)

### Services:
- `productService` - Product-related API calls
- `categoryService` - Category-related API calls  
- `cartService` - Shopping cart API calls

## 🛍️ Shopping Cart Features

- **Session-based Cart**: Uses browser localStorage for session management
- **Real-time Updates**: Instant cart updates with backend synchronization
- **Quantity Management**: Increase/decrease item quantities
- **Price Calculation**: Dynamic pricing with tax and shipping
- **Free Shipping**: Automatic free shipping over $500
- **Cart Persistence**: Cart survives browser refreshes

## 📱 Responsive Design

- **Mobile First**: Designed for mobile devices first
- **Breakpoints**: 
  - Mobile: < 640px
  - Tablet: 640px - 1024px
  - Desktop: > 1024px
- **Touch Friendly**: Large touch targets for mobile interaction
- **Grid Layouts**: Responsive product and category grids

## 🚀 Getting Started

### Prerequisites
- Node.js 16+ 
- npm or yarn
- .NET Backend API running on http://localhost:5009

### Installation

1. **Install Dependencies**:
   ```bash
   npm install --legacy-peer-deps
   ```

2. **Start Development Server**:
   ```bash
   npm start
   ```

3. **Access Application**:
   - Frontend: http://localhost:3000
   - Ensure backend API is running on http://localhost:5009

### Available Scripts

- `npm start` - Start development server
- `npm run build` - Build for production
- `npm test` - Run test suite
- `npm run eject` - Eject from Create React App

## 🌟 Key Features Implemented

### Homepage
- Hero section with call-to-action
- Featured products showcase
- Category browsing
- Customer testimonials
- Newsletter signup
- Feature highlights (shipping, warranty, returns)

### Product Catalog
- Product grid with filters and sorting
- Search functionality
- Category filtering
- Price range filtering
- Stock status indicators
- Hover effects and animations

### Product Details
- Image gallery with thumbnails
- Product specifications
- Quantity selector
- Add to cart functionality
- Star ratings and reviews
- Social sharing buttons

### Shopping Cart
- Cart item management
- Quantity updates
- Remove items
- Price calculations
- Checkout preparation
- Empty cart state

### Navigation
- Responsive header with mobile menu
- Cart icon with item count
- Breadcrumb navigation
- Footer with links and contact info

## 🔧 Configuration

### Environment Variables
Create a `.env` file in the root directory:

```env
REACT_APP_API_URL=http://localhost:5009/api
```

### Tailwind Configuration
The app uses a custom Tailwind configuration with:
- Custom color palette
- Extended font families
- Custom shadows and spacing
- Responsive breakpoints

## 🎯 Future Enhancements

- User authentication and accounts
- Product reviews and ratings
- Wishlist functionality
- Product recommendations
- Advanced search with filters
- Checkout and payment processing
- Order history and tracking
- Admin dashboard integration
- Progressive Web App (PWA) features
- Performance optimizations

## 📞 Support

For any issues or questions:
- Check the browser console for error messages
- Ensure the backend API is running
- Verify network connectivity between frontend and backend
- Check browser compatibility (modern browsers recommended)

## 🎉 Status

✅ **COMPLETE**: Frontend React application is fully functional and integrated with the backend API. The application is ready for development, testing, and production deployment.
