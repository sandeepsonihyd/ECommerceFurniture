// Product Image Utility Functions
// Helps map product data to local image paths

/**
 * Maps backend product image URLs to local paths
 * @param {Array} images - Array of product images from backend
 * @returns {Array} - Array with local image paths
 */
export const mapProductImages = (images = []) => {
  if (!images || images.length === 0) {
    return getDefaultProductImages();
  }

  return images.map(image => ({
    ...image,
    imageUrl: getLocalImagePath(image.imageUrl)
  }));
};

/**
 * Converts backend image URLs to local paths
 * @param {string} backendUrl - The backend image URL
 * @returns {string} - Local image path
 */
export const getLocalImagePath = (backendUrl) => {
  if (!backendUrl) return '/images/products/default-product.jpg';
  
  // Extract filename from backend URL pattern like "/images/sofa-modern-001-1.jpg"
  const filename = backendUrl.split('/').pop();
  
  // Map known backend filenames to local organized structure
  const imageMapping = {
    // Sofas
    'sofa-modern-001-1.jpg': '/images/products/sofas/sofa-modern-001-1.jpg',
    'sofa-modern-001-2.jpg': '/images/products/sofas/sofa-modern-001-2.jpg',
    'sofa-sectional-002-1.jpg': '/images/products/sofas/sofa-sectional-002-1.jpg',
    'sofa-leather-003-1.jpg': '/images/products/sofas/sofa-leather-003-1.jpg',
    
    // Tables
    'table-glass-001-1.jpg': '/images/products/tables/table-glass-001-1.jpg',
    'table-wood-002-1.jpg': '/images/products/tables/table-wood-002-1.jpg',
    'table-dining-003-1.jpg': '/images/products/tables/table-dining-003-1.jpg',
    'table-coffee-004-1.jpg': '/images/products/tables/table-coffee-004-1.jpg',
    
    // Chairs
    'chair-exec-001-1.jpg': '/images/products/chairs/chair-exec-001-1.jpg',
    'chair-dining-002-1.jpg': '/images/products/chairs/chair-dining-002-1.jpg',
    'chair-accent-003-1.jpg': '/images/products/chairs/chair-accent-003-1.jpg',
    'chair-office-004-1.jpg': '/images/products/chairs/chair-office-004-1.jpg',
    
    // Beds
    'bed-queen-001-1.jpg': '/images/products/beds/bed-queen-001-1.jpg',
    'bed-king-002-1.jpg': '/images/products/beds/bed-king-002-1.jpg',
    'bed-platform-003-1.jpg': '/images/products/beds/bed-platform-003-1.jpg',
    
    // Storage
    'dresser-001-1.jpg': '/images/products/storage/dresser-001-1.jpg',
    'bookshelf-002-1.jpg': '/images/products/storage/bookshelf-002-1.jpg',
    'wardrobe-003-1.jpg': '/images/products/storage/wardrobe-003-1.jpg',
    
    // Office
    'desk-001-1.jpg': '/images/products/office/desk-001-1.jpg',
    'filing-cabinet-002-1.jpg': '/images/products/office/filing-cabinet-002-1.jpg',
    'bookcase-003-1.jpg': '/images/products/office/bookcase-003-1.jpg'
  };

  // Return mapped path or fallback to organized structure
  return imageMapping[filename] || getProductImageByCategory(filename, backendUrl);
};

/**
 * Generate default product images for products without any images
 * @returns {Array} - Array of default image objects
 */
export const getDefaultProductImages = () => {
  return [
    {
      id: 0,
      imageUrl: '/images/products/default-product.jpg',
      altText: 'Product Image',
      isPrimary: true,
      displayOrder: 1
    }
  ];
};

/**
 * Determine product image path based on category or filename patterns
 * @param {string} filename - Image filename
 * @param {string} originalUrl - Original backend URL for reference
 * @returns {string} - Local image path
 */
export const getProductImageByCategory = (filename, originalUrl) => {
  const lowerFilename = filename.toLowerCase();
  
  // Determine category from filename patterns
  if (lowerFilename.includes('sofa') || lowerFilename.includes('couch')) {
    return `/images/products/sofas/${filename}`;
  }
  if (lowerFilename.includes('table') || lowerFilename.includes('desk')) {
    return `/images/products/tables/${filename}`;
  }
  if (lowerFilename.includes('chair') || lowerFilename.includes('seat')) {
    return `/images/products/chairs/${filename}`;
  }
  if (lowerFilename.includes('bed') || lowerFilename.includes('mattress')) {
    return `/images/products/beds/${filename}`;
  }
  if (lowerFilename.includes('storage') || lowerFilename.includes('dresser') || 
      lowerFilename.includes('wardrobe') || lowerFilename.includes('bookshelf')) {
    return `/images/products/storage/${filename}`;
  }
  if (lowerFilename.includes('office') || lowerFilename.includes('filing')) {
    return `/images/products/office/${filename}`;
  }
  
  // Default fallback
  return `/images/products/${filename}`;
};

/**
 * Get a sample product image for demos/fallbacks based on category
 * @param {string} categoryName - Category name
 * @returns {string} - Local image path
 */
export const getSampleImageByCategory = (categoryName) => {
  const categoryMapping = {
    'Sofas': '/images/products/sofas/sofa-modern-001-1.jpg',
    'Living Room': '/images/products/sofas/sofa-modern-001-1.jpg',
    'Tables': '/images/products/tables/table-glass-001-1.jpg',
    'Dining Room': '/images/products/tables/table-dining-003-1.jpg',
    'Chairs': '/images/products/chairs/chair-exec-001-1.jpg',
    'Office': '/images/products/chairs/chair-exec-001-1.jpg',
    'Beds': '/images/products/beds/bed-queen-001-1.jpg',
    'Bedroom': '/images/products/beds/bed-queen-001-1.jpg',
    'Storage': '/images/products/storage/dresser-001-1.jpg'
  };
  
  return categoryMapping[categoryName] || '/images/products/default-product.jpg';
};

/**
 * Process product data from API to use local images
 * @param {Object} product - Product object from API
 * @returns {Object} - Product with local image paths
 */
export const processProductForLocalImages = (product) => {
  if (!product) return product;
  
  return {
    ...product,
    images: mapProductImages(product.images)
  };
};

/**
 * Process array of products to use local images
 * @param {Array} products - Array of products from API
 * @returns {Array} - Products with local image paths
 */
export const processProductsForLocalImages = (products = []) => {
  return products.map(processProductForLocalImages);
}; 