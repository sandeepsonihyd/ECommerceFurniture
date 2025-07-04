import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Toaster } from 'react-hot-toast';
import { CartProvider } from './context/CartContext';
import { AuthProvider } from './context/AuthContext';

// Components
import Header from './components/common/Header';
import Footer from './components/common/Footer';

// Pages
import HomePage from './pages/HomePage';
import ProductsPage from './pages/ProductsPage';
import ProductDetailPage from './pages/ProductDetailPage';
import CategoriesPage from './pages/CategoriesPage';
import CartPage from './pages/CartPage';
import CSSDemo from './components/common/CSSDemo';

// About page component
const AboutPage = () => (
  <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
    <div className="text-center mb-12">
      <h1 className="text-4xl font-bold text-gray-900 mb-6">About FurnitureStore</h1>
      <p className="text-xl text-gray-600 max-w-3xl mx-auto">
        We're passionate about creating beautiful, functional furniture that transforms houses into homes.
      </p>
    </div>

    <div className="grid grid-cols-1 md:grid-cols-2 gap-12 items-center mb-16">
      <div>
        <h2 className="text-3xl font-bold text-gray-900 mb-6">Our Story</h2>
        <p className="text-gray-600 mb-4">
          Founded in 2020, FurnitureStore was born from a simple idea: everyone deserves beautiful, 
          quality furniture at affordable prices. We believe that your living space should reflect 
          your personality and lifestyle.
        </p>
        <p className="text-gray-600">
          Our team of designers and craftspeople work tirelessly to create pieces that are not only 
          stunning but also built to last. From modern minimalist designs to classic timeless pieces, 
          we have something for every taste and home.
        </p>
      </div>
      <div>
        <img
          src="/api/placeholder/500/400?text=Our+Showroom"
          alt="Our Showroom"
          className="rounded-lg shadow-soft"
        />
      </div>
    </div>

    <div className="bg-gray-50 rounded-lg p-8 mb-16">
      <h2 className="text-3xl font-bold text-gray-900 mb-8 text-center">Our Values</h2>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
        <div className="text-center">
          <h3 className="text-xl font-semibold text-gray-900 mb-4">Quality First</h3>
          <p className="text-gray-600">
            We never compromise on quality. Every piece is carefully crafted using premium materials 
            and traditional techniques.
          </p>
        </div>
        <div className="text-center">
          <h3 className="text-xl font-semibold text-gray-900 mb-4">Sustainable Design</h3>
          <p className="text-gray-600">
            We're committed to sustainable practices, using eco-friendly materials and responsible 
            manufacturing processes.
          </p>
        </div>
        <div className="text-center">
          <h3 className="text-xl font-semibold text-gray-900 mb-4">Customer Focused</h3>
          <p className="text-gray-600">
            Your satisfaction is our priority. We provide exceptional service from purchase to delivery 
            and beyond.
          </p>
        </div>
      </div>
    </div>

    <div className="text-center">
      <h2 className="text-3xl font-bold text-gray-900 mb-6">Visit Our Showroom</h2>
      <p className="text-lg text-gray-600 mb-4">
        Experience our furniture in person at our flagship showroom.
      </p>
      <p className="text-gray-600">
        123 Furniture Street, Design District<br />
        New York, NY 10001<br />
        (555) 123-4567
      </p>
    </div>
  </div>
);

function App() {
  return (
    <AuthProvider>
      <CartProvider>
        <Router>
          <div className="App min-h-screen flex flex-col bg-gray-50">
            <Header />
            
            <main className="flex-1">
              <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/products" element={<ProductsPage />} />
                <Route path="/products/:id" element={<ProductDetailPage />} />
                <Route path="/categories" element={<CategoriesPage />} />
                <Route path="/categories/:id" element={<ProductsPage />} />
                <Route path="/cart" element={<CartPage />} />
                <Route path="/about" element={<AboutPage />} />
                <Route path="/css-demo" element={<CSSDemo />} />
              </Routes>
            </main>
            
            <Footer />
            
            {/* Toast notifications */}
            <Toaster 
              position="top-right"
              toastOptions={{
                duration: 3000,
                style: {
                  background: '#fff',
                  color: '#333',
                  boxShadow: '0 4px 12px rgba(0, 0, 0, 0.15)',
                },
                success: {
                  iconTheme: {
                    primary: '#10B981',
                    secondary: '#fff',
                  },
                },
                error: {
                  iconTheme: {
                    primary: '#EF4444',
                    secondary: '#fff',
                  },
                },
              }}
            />
          </div>
        </Router>
      </CartProvider>
    </AuthProvider>
  );
}

export default App;
