import React from 'react';
import { Link } from 'react-router-dom';
import { ArrowRight } from 'lucide-react';

const CategoryCard = ({ category }) => {
  // Local category images
  const getCategoryImage = (categoryName) => {
    const imageMap = {
      'Living Room': '/images/categories/living-room.jpg',
      'Bedroom': '/images/categories/bedroom.jpg',
      'Dining Room': '/images/categories/dining-room.jpg',
      'Office': '/images/categories/office.jpg',
      'Sofas': '/images/categories/sofas.jpg',
      'Tables': '/images/categories/tables.jpg',
      'Kitchen': '/images/categories/kitchen.jpg',
      'Bathroom': '/images/categories/bathroom.jpg',
      'Outdoor': '/images/categories/outdoor.jpg',
      'Storage': '/images/categories/storage.jpg'
    };
    // Fallback to a default image if category not found
    return imageMap[categoryName] || '/images/categories/default.jpg';
  };

  return (
    <Link
      to={`/categories/${category.id}`}
      className="group relative overflow-hidden rounded-lg shadow-soft hover:shadow-lg transition-all duration-300 transform hover:-translate-y-1"
    >
      <div className="aspect-w-16 aspect-h-12 bg-gray-200">
        <img
          src={getCategoryImage(category.name)}
          alt={category.name}
          className="w-full h-64 object-cover object-center group-hover:scale-105 transition-transform duration-300"
        />
        <div className="absolute inset-0 bg-gradient-to-t from-black/60 to-transparent" />
      </div>

      <div className="absolute inset-0 flex flex-col justify-end p-6">
        <div className="text-white">
          <h3 className="text-xl font-bold mb-2 group-hover:text-secondary-300 transition-colors">
            {category.name}
          </h3>
          <p className="text-sm text-gray-200 mb-3 line-clamp-2">
            {category.description}
          </p>
          
          {/* Product Count */}
          {category.productCount !== undefined && (
            <p className="text-xs text-gray-300 mb-3">
              {category.productCount} products
            </p>
          )}

          {/* Subcategories */}
          {category.subCategories && category.subCategories.length > 0 && (
            <div className="mb-3">
              <p className="text-xs text-gray-300 mb-1">Categories:</p>
              <div className="flex flex-wrap gap-1">
                {category.subCategories.slice(0, 3).map((sub) => (
                  <span
                    key={sub.id}
                    className="text-xs bg-white/20 backdrop-blur-sm rounded px-2 py-1"
                  >
                    {sub.name}
                  </span>
                ))}
                {category.subCategories.length > 3 && (
                  <span className="text-xs text-gray-300">
                    +{category.subCategories.length - 3} more
                  </span>
                )}
              </div>
            </div>
          )}

          <div className="flex items-center text-sm font-medium group-hover:text-secondary-300 transition-colors">
            Shop Now
            <ArrowRight size={16} className="ml-2 group-hover:translate-x-1 transition-transform" />
          </div>
        </div>
      </div>
    </Link>
  );
};

export default CategoryCard; 