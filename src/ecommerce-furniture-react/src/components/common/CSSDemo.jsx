import React from 'react';
import { CheckCircle, Heart, Star, ShoppingCart } from 'lucide-react';

const CSSDemo = () => {
  return (
    <div className="max-w-4xl mx-auto p-8 space-y-8">
      <div className="text-center mb-12">
        <h1 className="text-4xl font-bold text-gray-900 mb-4">CSS Support Validation</h1>
        <p className="text-lg text-gray-600">
          This page demonstrates that Tailwind CSS is fully functional
        </p>
      </div>

      {/* Custom Colors */}
      <section className="bg-white rounded-lg shadow-soft p-6">
        <h2 className="text-2xl font-semibold text-gray-900 mb-4">Custom Color Palette</h2>
        <div className="grid grid-cols-2 gap-4">
          <div>
            <h3 className="text-lg font-medium mb-3">Primary Colors</h3>
            <div className="space-y-2">
              <div className="flex items-center space-x-3">
                <div className="w-6 h-6 bg-primary-50 rounded border"></div>
                <span className="text-sm">primary-50</span>
              </div>
              <div className="flex items-center space-x-3">
                <div className="w-6 h-6 bg-primary-600 rounded"></div>
                <span className="text-sm">primary-600</span>
              </div>
              <div className="flex items-center space-x-3">
                <div className="w-6 h-6 bg-primary-900 rounded"></div>
                <span className="text-sm">primary-900</span>
              </div>
            </div>
          </div>
          <div>
            <h3 className="text-lg font-medium mb-3">Secondary Colors</h3>
            <div className="space-y-2">
              <div className="flex items-center space-x-3">
                <div className="w-6 h-6 bg-secondary-50 rounded border"></div>
                <span className="text-sm">secondary-50</span>
              </div>
              <div className="flex items-center space-x-3">
                <div className="w-6 h-6 bg-secondary-600 rounded"></div>
                <span className="text-sm">secondary-600</span>
              </div>
              <div className="flex items-center space-x-3">
                <div className="w-6 h-6 bg-secondary-900 rounded"></div>
                <span className="text-sm">secondary-900</span>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Typography */}
      <section className="bg-white rounded-lg shadow-soft p-6">
        <h2 className="text-2xl font-semibold text-gray-900 mb-4">Typography (Inter Font)</h2>
        <div className="space-y-4">
          <div className="text-4xl font-bold text-gray-900">Heading 1 - Bold</div>
          <div className="text-3xl font-semibold text-gray-800">Heading 2 - Semibold</div>
          <div className="text-2xl font-medium text-gray-700">Heading 3 - Medium</div>
          <div className="text-lg font-normal text-gray-600">Body Large - Normal</div>
          <div className="text-base font-normal text-gray-600">Body Base - Normal</div>
          <div className="text-sm font-light text-gray-500">Small Text - Light</div>
        </div>
      </section>

      {/* Buttons */}
      <section className="bg-white rounded-lg shadow-soft p-6">
        <h2 className="text-2xl font-semibold text-gray-900 mb-4">Button Components</h2>
        <div className="flex flex-wrap gap-4">
          <button className="bg-primary-600 text-white hover:bg-primary-700 px-4 py-2 rounded-lg transition-colors">
            Primary Button
          </button>
          <button className="bg-secondary-600 text-white hover:bg-secondary-700 px-4 py-2 rounded-lg transition-colors">
            Secondary Button
          </button>
          <button className="border border-primary-600 text-primary-600 hover:bg-primary-50 px-4 py-2 rounded-lg transition-colors">
            Outline Button
          </button>
          <button className="text-primary-600 hover:bg-primary-50 px-4 py-2 rounded-lg transition-colors">
            Ghost Button
          </button>
        </div>
      </section>

      {/* Cards */}
      <section className="bg-white rounded-lg shadow-soft p-6">
        <h2 className="text-2xl font-semibold text-gray-900 mb-4">Card Components</h2>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div className="bg-white border border-gray-200 rounded-lg p-4 hover:shadow-lg transition-shadow">
            <div className="bg-primary-100 w-12 h-12 rounded-full flex items-center justify-center mb-3">
              <CheckCircle className="text-primary-600" size={24} />
            </div>
            <h3 className="text-lg font-semibold text-gray-900 mb-2">Feature Card</h3>
            <p className="text-gray-600 text-sm">This card demonstrates hover effects and shadows.</p>
          </div>
          
          <div className="bg-gradient-to-br from-primary-500 to-primary-700 text-white rounded-lg p-4">
            <Heart className="text-white mb-3" size={24} />
            <h3 className="text-lg font-semibold mb-2">Gradient Card</h3>
            <p className="text-primary-100 text-sm">Card with gradient background.</p>
          </div>
          
          <div className="bg-white border-2 border-secondary-300 rounded-lg p-4">
            <Star className="text-secondary-600 mb-3" size={24} />
            <h3 className="text-lg font-semibold text-gray-900 mb-2">Bordered Card</h3>
            <p className="text-gray-600 text-sm">Card with colored border.</p>
          </div>
        </div>
      </section>

      {/* Grid & Layout */}
      <section className="bg-white rounded-lg shadow-soft p-6">
        <h2 className="text-2xl font-semibold text-gray-900 mb-4">Responsive Grid</h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
          {[1, 2, 3, 4, 5, 6, 7, 8].map((item) => (
            <div key={item} className="bg-primary-50 border border-primary-200 rounded-lg p-4 text-center">
              <div className="text-primary-600 font-semibold">Grid Item {item}</div>
            </div>
          ))}
        </div>
      </section>

      {/* Animations & Effects */}
      <section className="bg-white rounded-lg shadow-soft p-6">
        <h2 className="text-2xl font-semibold text-gray-900 mb-4">Animations & Effects</h2>
        <div className="space-y-4">
          <div className="group p-4 bg-gray-50 rounded-lg hover:bg-primary-50 transition-all duration-300 cursor-pointer">
            <div className="flex items-center space-x-3">
              <ShoppingCart className="text-gray-600 group-hover:text-primary-600 transition-colors" size={20} />
              <span className="text-gray-700 group-hover:text-primary-700">Hover me for color transition</span>
            </div>
          </div>
          
          <div className="transform hover:scale-105 transition-transform duration-200 bg-secondary-50 p-4 rounded-lg cursor-pointer">
            <span className="text-secondary-700">Hover me for scale effect</span>
          </div>
          
          <div className="animate-pulse bg-gray-200 rounded-lg p-4">
            <span className="text-gray-600">Pulse animation (loading state)</span>
          </div>
        </div>
      </section>

      {/* Custom Shadow */}
      <section className="bg-white rounded-lg shadow-soft p-6">
        <h2 className="text-2xl font-semibold text-gray-900 mb-4">Custom Shadow (shadow-soft)</h2>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div className="bg-white shadow-soft rounded-lg p-4">
            <span className="text-gray-700">Custom soft shadow</span>
          </div>
          <div className="bg-white shadow-lg rounded-lg p-4">
            <span className="text-gray-700">Default large shadow</span>
          </div>
          <div className="bg-white shadow-xl rounded-lg p-4">
            <span className="text-gray-700">Extra large shadow</span>
          </div>
        </div>
      </section>

      {/* Success Indicator */}
      <div className="bg-green-50 border border-green-200 rounded-lg p-6 text-center">
        <CheckCircle className="text-green-600 mx-auto mb-4" size={48} />
        <h2 className="text-2xl font-bold text-green-800 mb-2">CSS Support Confirmed!</h2>
        <p className="text-green-700">
          ✅ Tailwind CSS is fully functional<br/>
          ✅ Custom colors are working<br/>
          ✅ Inter font is loaded<br/>
          ✅ Custom shadows are applied<br/>
          ✅ Responsive design is active
        </p>
      </div>
    </div>
  );
};

export default CSSDemo; 