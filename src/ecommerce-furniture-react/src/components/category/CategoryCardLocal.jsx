// Example: Using Local Images
const getCategoryImage = (categoryName) => {
  const imageMap = {
    'Living Room': '/images/categories/living-room.jpg',
    'Bedroom': '/images/categories/bedroom.jpg',
    'Dining Room': '/images/categories/dining-room.jpg',
    'Office': '/images/categories/office.jpg',
    'Sofas': '/images/categories/sofas.jpg',
    'Tables': '/images/categories/tables.jpg'
  };
  return imageMap[categoryName] || '/images/categories/default.jpg';
};

// To use local images:
// 1. Place images in public/images/categories/
// 2. Name them: bedroom.jpg, living-room.jpg, etc.
// 3. Update the CategoryCard component with the above function 