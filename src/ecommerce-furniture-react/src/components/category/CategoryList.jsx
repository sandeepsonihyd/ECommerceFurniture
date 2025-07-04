import React from 'react';
import CategoryCard from './CategoryCard';
import Loading from '../common/Loading';

const CategoryList = ({ categories, loading, title = "Shop by Category" }) => {
  if (loading) {
    return <Loading size="large" text="Loading categories..." />;
  }

  if (!categories || categories.length === 0) {
    return (
      <div className="text-center py-12">
        <p className="text-gray-500">No categories available.</p>
      </div>
    );
  }

  // Filter to show only parent categories for main category page
  const mainCategories = categories.filter(cat => !cat.parentCategoryId);

  return (
    <div className="space-y-8">
      <div className="text-center">
        <h2 className="text-3xl font-bold text-gray-900 mb-4">{title}</h2>
        <p className="text-lg text-gray-600 max-w-2xl mx-auto">
          Discover our carefully curated collection of furniture for every room in your home.
        </p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        {mainCategories.map((category) => (
          <CategoryCard key={category.id} category={category} />
        ))}
      </div>

      {/* Featured Subcategories */}
      {categories.some(cat => cat.parentCategoryId) && (
        <div className="mt-16">
          <h3 className="text-2xl font-bold text-gray-900 mb-8 text-center">
            Popular Subcategories
          </h3>
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
            {categories
              .filter(cat => cat.parentCategoryId)
              .slice(0, 4)
              .map((category) => (
                <div
                  key={category.id}
                  className="bg-white rounded-lg shadow-soft p-4 text-center hover:shadow-lg transition-shadow"
                >
                  <h4 className="font-semibold text-gray-900">{category.name}</h4>
                  <p className="text-sm text-gray-600 mt-1">{category.description}</p>
                  {category.productCount !== undefined && (
                    <p className="text-xs text-gray-400 mt-2">
                      {category.productCount} products
                    </p>
                  )}
                </div>
              ))}
          </div>
        </div>
      )}
    </div>
  );
};

export default CategoryList; 