using Moq;
using FluentAssertions;
using ECommerceFurniture.Business.Services;
using ECommerceFurniture.Business.DTOs;
using ECommerceFurniture.Domain;
using ECommerceFurniture.Repository;

namespace ECommerceFurniture.Business.Tests;

public class CategoryServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCategoryRepository = new Mock<ICategoryRepository>();

        _mockUnitOfWork.Setup(u => u.Categories).Returns(_mockCategoryRepository.Object);

        _categoryService = new CategoryService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenCategoriesExist_ShouldReturnAllCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category
            {
                Id = 1,
                Name = "Living Room",
                Description = "Living room furniture",
                IsActive = true,
                ParentCategoryId = null,
                SubCategories = new List<Category>(),
                Products = new List<Product> { new Product(), new Product() }
            },
            new Category
            {
                Id = 2,
                Name = "Bedroom",
                Description = "Bedroom furniture",
                IsActive = true,
                ParentCategoryId = null,
                SubCategories = new List<Category>(),
                Products = new List<Product> { new Product() }
            }
        };

        _mockCategoryRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _categoryService.GetAllCategoriesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        
        var resultList = result.ToList();
        resultList[0].Name.Should().Be("Living Room");
        resultList[0].ProductCount.Should().Be(2);
        resultList[1].Name.Should().Be("Bedroom");
        resultList[1].ProductCount.Should().Be(1);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_WhenNoCategoriesExist_ShouldReturnEmptyList()
    {
        // Arrange
        _mockCategoryRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Category>());

        // Act
        var result = await _categoryService.GetAllCategoriesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryExists_ShouldReturnCategoryDto()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category
        {
            Id = categoryId,
            Name = "Living Room",
            Description = "Living room furniture",
            IsActive = true,
            ParentCategoryId = null,
            ParentCategory = null,
            SubCategories = new List<Category>
            {
                new Category
                {
                    Id = 3,
                    Name = "Sofas",
                    Description = "Comfortable sofas",
                    IsActive = true,
                    ParentCategoryId = categoryId
                }
            },
            Products = new List<Product> { new Product(), new Product(), new Product() }
        };

        _mockCategoryRepository.Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync(category);

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(categoryId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(categoryId);
        result.Name.Should().Be("Living Room");
        result.Description.Should().Be("Living room furniture");
        result.IsActive.Should().BeTrue();
        result.ParentCategoryId.Should().BeNull();
        result.ParentCategoryName.Should().BeNull();
        result.SubCategories.Should().HaveCount(1);
        result.SubCategories.First().Name.Should().Be("Sofas");
        result.ProductCount.Should().Be(3);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryNotFound_ShouldReturnNull()
    {
        // Arrange
        var categoryId = 999;

        _mockCategoryRepository.Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync((Category?)null);

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(categoryId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetActiveCategoriesAsync_WhenActiveCategoriesExist_ShouldReturnOnlyActiveCategories()
    {
        // Arrange
        var activeCategories = new List<Category>
        {
            new Category
            {
                Id = 1,
                Name = "Living Room",
                Description = "Living room furniture",
                IsActive = true,
                ParentCategoryId = null,
                SubCategories = new List<Category>(),
                Products = new List<Product> { new Product() }
            },
            new Category
            {
                Id = 2,
                Name = "Bedroom",
                Description = "Bedroom furniture",
                IsActive = true,
                ParentCategoryId = null,
                SubCategories = new List<Category>(),
                Products = new List<Product> { new Product(), new Product() }
            }
        };

        _mockCategoryRepository.Setup(x => x.GetActiveCategoriesAsync())
            .ReturnsAsync(activeCategories);

        // Act
        var result = await _categoryService.GetActiveCategoriesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(c => c.IsActive);
        
        var resultList = result.ToList();
        resultList[0].Name.Should().Be("Living Room");
        resultList[0].ProductCount.Should().Be(1);
        resultList[1].Name.Should().Be("Bedroom");
        resultList[1].ProductCount.Should().Be(2);
    }

    [Fact]
    public async Task GetActiveCategoriesAsync_WhenNoActiveCategoriesExist_ShouldReturnEmptyList()
    {
        // Arrange
        _mockCategoryRepository.Setup(x => x.GetActiveCategoriesAsync())
            .ReturnsAsync(new List<Category>());

        // Act
        var result = await _categoryService.GetActiveCategoriesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
} 