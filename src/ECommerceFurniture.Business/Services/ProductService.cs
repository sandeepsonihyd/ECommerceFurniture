using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceFurniture.Business.DTOs;
using ECommerceFurniture.Domain;
using ECommerceFurniture.Repository;

namespace ECommerceFurniture.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetProductWithDetailsAsync(id);
            return product != null ? MapToProductDto(product) : null;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return products.Select(MapToProductDto);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _unitOfWork.Products.GetProductsByCategoryAsync(categoryId);
            return products.Select(MapToProductDto);
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            var products = await _unitOfWork.Products.SearchProductsAsync(searchTerm);
            return products.Select(MapToProductDto);
        }

        public async Task<IEnumerable<ProductDto>> GetFeaturedProductsAsync()
        {
            var products = await _unitOfWork.Products.GetFeaturedProductsAsync();
            return products.Select(MapToProductDto);
        }

        public async Task<PagedResult<ProductDto>> GetProductsPagedAsync(ProductFilterDto filter)
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            
            // Apply filters
            var filteredProducts = products.AsQueryable();

            if (filter.CategoryId.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.CategoryId == filter.CategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                filteredProducts = filteredProducts.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Description.ToLower().Contains(searchTerm) ||
                    p.SKU.ToLower().Contains(searchTerm));
            }

            if (filter.MinPrice.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            var totalCount = filteredProducts.Count();
            var pagedProducts = filteredProducts
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new PagedResult<ProductDto>
            {
                Items = pagedProducts.Select(MapToProductDto),
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        private static ProductDto MapToProductDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU,
                CategoryName = product.Category?.Name ?? "",
                StockQuantity = product.StockQuantity,
                Images = product.ProductImages?.Select(img => new ProductImageDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    AltText = img.AltText,
                    IsPrimary = img.IsPrimary,
                    DisplayOrder = img.DisplayOrder
                }).ToList() ?? new List<ProductImageDto>(),
                Specifications = product.Specifications?.Select(spec => new ProductSpecificationDto
                {
                    Id = spec.Id,
                    Name = spec.Name,
                    Value = spec.Value,
                    DisplayOrder = spec.DisplayOrder
                }).ToList() ?? new List<ProductSpecificationDto>()
            };
        }
    }
} 