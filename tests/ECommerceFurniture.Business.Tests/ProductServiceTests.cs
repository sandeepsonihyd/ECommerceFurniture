using Moq;
using FluentAssertions;
using ECommerceFurniture.Business.Services;
using ECommerceFurniture.Business.DTOs;
using ECommerceFurniture.Domain;
using ECommerceFurniture.Repository;

namespace ECommerceFurniture.Business.Tests;

public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IProductRepository>();

        _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);

        _productService = new ProductService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductExists_ShouldReturnProductDto()
    {
        // Arrange
        var productId = 1;
        var category = new Category { Id = 1, Name = "Living Room" };
        var product = new Product
        {
            Id = productId,
            Name = "Modern Sofa",
            Description = "A comfortable modern sofa",
            Price = 899.99m,
            SKU = "SOF001",
            CategoryId = 1,
            Category = category,
            StockQuantity = 5,
            ProductImages = new List<ProductImage>
            {
                new ProductImage { Id = 1, ImageUrl = "image1.jpg", IsPrimary = true, DisplayOrder = 1 },
                new ProductImage { Id = 2, ImageUrl = "image2.jpg", IsPrimary = false, DisplayOrder = 2 }
            },
            Specifications = new List<ProductSpecification>
            {
                new ProductSpecification { Id = 1, Name = "Material", Value = "Leather", DisplayOrder = 1 },
                new ProductSpecification { Id = 2, Name = "Dimensions", Value = "200x90x80 cm", DisplayOrder = 2 }
            }
        };

        _mockProductRepository.Setup(x => x.GetProductWithDetailsAsync(productId))
            .ReturnsAsync(product);

        // Act
        var result = await _productService.GetProductByIdAsync(productId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(productId);
        result.Name.Should().Be("Modern Sofa");
        result.Description.Should().Be("A comfortable modern sofa");
        result.Price.Should().Be(899.99m);
        result.SKU.Should().Be("SOF001");
        result.CategoryName.Should().Be("Living Room");
        result.StockQuantity.Should().Be(5);
        result.Images.Should().HaveCount(2);
        result.Images.First().IsPrimary.Should().BeTrue();
        result.Specifications.Should().HaveCount(2);
        result.Specifications.First().Name.Should().Be("Material");
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductNotFound_ShouldReturnNull()
    {
        // Arrange
        var productId = 999;

        _mockProductRepository.Setup(x => x.GetProductWithDetailsAsync(productId))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _productService.GetProductByIdAsync(productId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllProductsAsync_WhenProductsExist_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Modern Sofa",
                Description = "A comfortable modern sofa",
                Price = 899.99m,
                SKU = "SOF001",
                Category = new Category { Name = "Living Room" },
                StockQuantity = 5,
                ProductImages = new List<ProductImage>(),
                Specifications = new List<ProductSpecification>()
            },
            new Product
            {
                Id = 2,
                Name = "Office Chair",
                Description = "Ergonomic office chair",
                Price = 299.99m,
                SKU = "CHR001",
                Category = new Category { Name = "Office" },
                StockQuantity = 10,
                ProductImages = new List<ProductImage>(),
                Specifications = new List<ProductSpecification>()
            }
        };

        _mockProductRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        
        var resultList = result.ToList();
        resultList[0].Name.Should().Be("Modern Sofa");
        resultList[0].CategoryName.Should().Be("Living Room");
        resultList[1].Name.Should().Be("Office Chair");
        resultList[1].CategoryName.Should().Be("Office");
    }

    [Fact]
    public async Task GetAllProductsAsync_WhenNoProductsExist_ShouldReturnEmptyList()
    {
        // Arrange
        _mockProductRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Product>());

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetProductsByCategoryAsync_WhenCategoryHasProducts_ShouldReturnCategoryProducts()
    {
        // Arrange
        var categoryId = 1;
        var categoryProducts = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Modern Sofa",
                CategoryId = categoryId,
                Category = new Category { Name = "Living Room" },
                Price = 899.99m,
                ProductImages = new List<ProductImage>(),
                Specifications = new List<ProductSpecification>()
            },
            new Product
            {
                Id = 2,
                Name = "Coffee Table",
                CategoryId = categoryId,
                Category = new Category { Name = "Living Room" },
                Price = 299.99m,
                ProductImages = new List<ProductImage>(),
                Specifications = new List<ProductSpecification>()
            }
        };

        _mockProductRepository.Setup(x => x.GetProductsByCategoryAsync(categoryId))
            .ReturnsAsync(categoryProducts);

        // Act
        var result = await _productService.GetProductsByCategoryAsync(categoryId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(p => p.CategoryName == "Living Room");
    }

    [Fact]
    public async Task SearchProductsAsync_WhenSearchTermMatches_ShouldReturnMatchingProducts()
    {
        // Arrange
        var searchTerm = "sofa";
        var searchResults = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Modern Sofa",
                Description = "A comfortable modern sofa",
                SKU = "SOF001",
                Category = new Category { Name = "Living Room" },
                ProductImages = new List<ProductImage>(),
                Specifications = new List<ProductSpecification>()
            },
            new Product
            {
                Id = 2,
                Name = "Sectional Sofa",
                Description = "Large sectional sofa",
                SKU = "SOF002",
                Category = new Category { Name = "Living Room" },
                ProductImages = new List<ProductImage>(),
                Specifications = new List<ProductSpecification>()
            }
        };

        _mockProductRepository.Setup(x => x.SearchProductsAsync(searchTerm))
            .ReturnsAsync(searchResults);

        // Act
        var result = await _productService.SearchProductsAsync(searchTerm);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(p => p.Name.ToLower().Contains("sofa"));
    }

    [Fact]
    public async Task GetFeaturedProductsAsync_WhenFeaturedProductsExist_ShouldReturnFeaturedProducts()
    {
        // Arrange
        var featuredProducts = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Premium Sofa",
                StockQuantity = 15,
                Category = new Category { Name = "Living Room" },
                ProductImages = new List<ProductImage>(),
                Specifications = new List<ProductSpecification>()
            },
            new Product
            {
                Id = 2,
                Name = "Designer Chair",
                StockQuantity = 20,
                Category = new Category { Name = "Office" },
                ProductImages = new List<ProductImage>(),
                Specifications = new List<ProductSpecification>()
            }
        };

        _mockProductRepository.Setup(x => x.GetFeaturedProductsAsync())
            .ReturnsAsync(featuredProducts);

        // Act
        var result = await _productService.GetFeaturedProductsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Name == "Premium Sofa");
        result.Should().Contain(p => p.Name == "Designer Chair");
    }

    [Fact]
    public async Task GetProductsPagedAsync_WithCategoryFilter_ShouldReturnFilteredProducts()
    {
        // Arrange
        var filter = new ProductFilterDto
        {
            CategoryId = 1,
            PageNumber = 1,
            PageSize = 2
        };

        var allProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Sofa", CategoryId = 1, Price = 500m, Category = new Category { Name = "Living Room" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() },
            new Product { Id = 2, Name = "Table", CategoryId = 1, Price = 300m, Category = new Category { Name = "Living Room" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() },
            new Product { Id = 3, Name = "Chair", CategoryId = 2, Price = 200m, Category = new Category { Name = "Office" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() }
        };

        _mockProductRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(allProducts);

        // Act
        var result = await _productService.GetProductsPagedAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(2);
        result.Items.Should().OnlyContain(p => p.CategoryName == "Living Room");
    }

    [Fact]
    public async Task GetProductsPagedAsync_WithSearchFilter_ShouldReturnMatchingProducts()
    {
        // Arrange
        var filter = new ProductFilterDto
        {
            SearchTerm = "chair",
            PageNumber = 1,
            PageSize = 10
        };

        var allProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Office Chair", Description = "Ergonomic chair", SKU = "CHR001", Category = new Category { Name = "Office" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() },
            new Product { Id = 2, Name = "Dining Table", Description = "Wooden table", SKU = "TBL001", Category = new Category { Name = "Dining" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() },
            new Product { Id = 3, Name = "Gaming Chair", Description = "Comfortable gaming chair", SKU = "CHR002", Category = new Category { Name = "Office" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() }
        };

        _mockProductRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(allProducts);

        // Act
        var result = await _productService.GetProductsPagedAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.Should().OnlyContain(p => 
            p.Name.ToLower().Contains("chair") || 
            p.Description.ToLower().Contains("chair"));
    }

    [Fact]
    public async Task GetProductsPagedAsync_WithPriceFilter_ShouldReturnProductsInPriceRange()
    {
        // Arrange
        var filter = new ProductFilterDto
        {
            MinPrice = 200m,
            MaxPrice = 500m,
            PageNumber = 1,
            PageSize = 10
        };

        var allProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Cheap Chair", Price = 100m, Category = new Category { Name = "Office" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() },
            new Product { Id = 2, Name = "Mid Range Table", Price = 300m, Category = new Category { Name = "Dining" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() },
            new Product { Id = 3, Name = "Expensive Sofa", Price = 800m, Category = new Category { Name = "Living Room" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() },
            new Product { Id = 4, Name = "Good Chair", Price = 250m, Category = new Category { Name = "Office" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() }
        };

        _mockProductRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(allProducts);

        // Act
        var result = await _productService.GetProductsPagedAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.Should().OnlyContain(p => p.Price >= 200m && p.Price <= 500m);
    }

    [Fact]
    public async Task GetProductsPagedAsync_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var filter = new ProductFilterDto
        {
            PageNumber = 2,
            PageSize = 2
        };

        var allProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Category = new Category { Name = "Category1" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() },
            new Product { Id = 2, Name = "Product 2", Category = new Category { Name = "Category1" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() },
            new Product { Id = 3, Name = "Product 3", Category = new Category { Name = "Category1" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() },
            new Product { Id = 4, Name = "Product 4", Category = new Category { Name = "Category1" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() },
            new Product { Id = 5, Name = "Product 5", Category = new Category { Name = "Category1" }, ProductImages = new List<ProductImage>(), Specifications = new List<ProductSpecification>() }
        };

        _mockProductRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(allProducts);

        // Act
        var result = await _productService.GetProductsPagedAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(5);
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(2);
        
        var items = result.Items.ToList();
        items[0].Name.Should().Be("Product 3");
        items[1].Name.Should().Be("Product 4");
    }
} 