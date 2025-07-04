using Moq;
using FluentAssertions;
using ECommerceFurniture.Business.Services;
using ECommerceFurniture.Business.DTOs;
using ECommerceFurniture.Domain;
using ECommerceFurniture.Repository;

namespace ECommerceFurniture.Business.Tests;

public class CartServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICartRepository> _mockCartRepository;
    private readonly Mock<ICartItemRepository> _mockCartItemRepository;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly CartService _cartService;

    public CartServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCartRepository = new Mock<ICartRepository>();
        _mockCartItemRepository = new Mock<ICartItemRepository>();
        _mockProductRepository = new Mock<IProductRepository>();

        _mockUnitOfWork.Setup(u => u.Carts).Returns(_mockCartRepository.Object);
        _mockUnitOfWork.Setup(u => u.CartItems).Returns(_mockCartItemRepository.Object);
        _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);

        _cartService = new CartService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetCartAsync_WhenCartExists_ShouldReturnCartDto()
    {
        // Arrange
        var sessionId = "test-session-123";
        var cart = new Cart
        {
            Id = 1,
            SessionId = sessionId,
            CreatedDate = DateTime.UtcNow,
            CartItems = new List<CartItem>()
        };

        _mockCartRepository.Setup(x => x.GetCartWithItemsAsync(sessionId))
            .ReturnsAsync(cart);

        // Act
        var result = await _cartService.GetCartAsync(sessionId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cart.Id);
        result.SessionId.Should().Be(sessionId);
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCartAsync_WhenCartDoesNotExist_ShouldCreateNewCart()
    {
        // Arrange
        var sessionId = "new-session-123";
        
        _mockCartRepository.Setup(x => x.GetCartWithItemsAsync(sessionId))
            .ReturnsAsync((Cart?)null);

        _mockCartRepository.Setup(x => x.AddAsync(It.IsAny<Cart>()))
            .Returns(Task.FromResult(new Cart()));

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _cartService.GetCartAsync(sessionId);

        // Assert
        result.Should().NotBeNull();
        result.SessionId.Should().Be(sessionId);
        result.Items.Should().BeEmpty();

        _mockCartRepository.Verify(x => x.AddAsync(It.Is<Cart>(c => c.SessionId == sessionId)), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddToCartAsync_WhenProductExists_ShouldAddNewCartItem()
    {
        // Arrange
        var addToCartDto = new AddToCartDto
        {
            SessionId = "test-session",
            ProductId = 1,
            Quantity = 2
        };

        var cart = new Cart { Id = 1, SessionId = addToCartDto.SessionId };
        var product = new Product { Id = 1, Name = "Test Product", Price = 99.99m };
        var addedCartItem = new CartItem
        {
            Id = 1,
            CartId = cart.Id,
            ProductId = addToCartDto.ProductId,
            Quantity = addToCartDto.Quantity,
            UnitPrice = product.Price,
            Product = product
        };

        _mockCartRepository.Setup(x => x.GetCartBySessionIdAsync(addToCartDto.SessionId))
            .ReturnsAsync(cart);

        _mockProductRepository.Setup(x => x.GetByIdAsync(addToCartDto.ProductId))
            .ReturnsAsync(product);

        _mockCartItemRepository.SetupSequence(x => x.GetCartItemByCartAndProductAsync(cart.Id, addToCartDto.ProductId))
            .ReturnsAsync((CartItem?)null)
            .ReturnsAsync(addedCartItem);

        _mockCartItemRepository.Setup(x => x.AddAsync(It.IsAny<CartItem>()))
            .Returns(Task.FromResult(new CartItem()));

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _cartService.AddToCartAsync(addToCartDto);

        // Assert
        result.Should().NotBeNull();
        result.ProductId.Should().Be(addToCartDto.ProductId);
        result.Quantity.Should().Be(addedCartItem.Quantity);
        result.UnitPrice.Should().Be(product.Price);

        _mockCartItemRepository.Verify(x => x.AddAsync(It.IsAny<CartItem>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddToCartAsync_WhenCartItemAlreadyExists_ShouldUpdateQuantity()
    {
        // Arrange
        var addToCartDto = new AddToCartDto
        {
            SessionId = "test-session",
            ProductId = 1,
            Quantity = 2
        };

        var cart = new Cart { Id = 1, SessionId = addToCartDto.SessionId };
        var product = new Product { Id = 1, Name = "Test Product", Price = 99.99m };
        var existingCartItem = new CartItem
        {
            Id = 1,
            CartId = cart.Id,
            ProductId = addToCartDto.ProductId,
            Quantity = 1,
            UnitPrice = product.Price,
            Product = product
        };

        _mockCartRepository.Setup(x => x.GetCartBySessionIdAsync(addToCartDto.SessionId))
            .ReturnsAsync(cart);

        _mockProductRepository.Setup(x => x.GetByIdAsync(addToCartDto.ProductId))
            .ReturnsAsync(product);

        _mockCartItemRepository.Setup(x => x.GetCartItemByCartAndProductAsync(cart.Id, addToCartDto.ProductId))
            .ReturnsAsync(existingCartItem);

        _mockCartItemRepository.Setup(x => x.Update(It.IsAny<CartItem>()));

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _cartService.AddToCartAsync(addToCartDto);

        // Assert
        result.Should().NotBeNull();
        result.Quantity.Should().Be(3); // 1 + 2

        _mockCartItemRepository.Verify(x => x.Update(It.Is<CartItem>(ci => ci.Quantity == 3)), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task AddToCartAsync_WhenProductNotFound_ShouldThrowArgumentException()
    {
        // Arrange
        var addToCartDto = new AddToCartDto
        {
            SessionId = "test-session",
            ProductId = 999,
            Quantity = 1
        };

        var cart = new Cart { Id = 1, SessionId = addToCartDto.SessionId };

        _mockCartRepository.Setup(x => x.GetCartBySessionIdAsync(addToCartDto.SessionId))
            .ReturnsAsync(cart);

        _mockProductRepository.Setup(x => x.GetByIdAsync(addToCartDto.ProductId))
            .ReturnsAsync((Product?)null);

        // Act & Assert
        await _cartService.Invoking(x => x.AddToCartAsync(addToCartDto))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Product not found*");
    }

    [Fact]
    public async Task UpdateCartItemAsync_WhenCartItemExists_ShouldUpdateQuantity()
    {
        // Arrange
        var updateDto = new UpdateCartItemDto
        {
            CartItemId = 1,
            Quantity = 5
        };

        var cartItem = new CartItem
        {
            Id = 1,
            CartId = 1,
            ProductId = 1,
            Quantity = 2,
            UnitPrice = 99.99m,
            Product = new Product { Id = 1, Name = "Test Product" }
        };

        _mockCartItemRepository.Setup(x => x.GetByIdAsync(updateDto.CartItemId))
            .ReturnsAsync(cartItem);

        _mockCartItemRepository.Setup(x => x.Update(It.IsAny<CartItem>()));

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _cartService.UpdateCartItemAsync(updateDto);

        // Assert
        result.Should().NotBeNull();
        result!.Quantity.Should().Be(5);

        _mockCartItemRepository.Verify(x => x.Update(It.Is<CartItem>(ci => ci.Quantity == 5)), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateCartItemAsync_WhenQuantityIsZeroOrLess_ShouldRemoveItem()
    {
        // Arrange
        var updateDto = new UpdateCartItemDto
        {
            CartItemId = 1,
            Quantity = 0
        };

        var cartItem = new CartItem
        {
            Id = 1,
            CartId = 1,
            ProductId = 1,
            Quantity = 2,
            UnitPrice = 99.99m
        };

        _mockCartItemRepository.Setup(x => x.GetByIdAsync(updateDto.CartItemId))
            .ReturnsAsync(cartItem);

        _mockCartItemRepository.Setup(x => x.Remove(It.IsAny<CartItem>()));

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _cartService.UpdateCartItemAsync(updateDto);

        // Assert
        result.Should().BeNull();

        _mockCartItemRepository.Verify(x => x.Remove(cartItem), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RemoveFromCartAsync_WhenCartItemExists_ShouldRemoveAndReturnTrue()
    {
        // Arrange
        var cartItemId = 1;
        var cartItem = new CartItem { Id = cartItemId };

        _mockCartItemRepository.Setup(x => x.GetByIdAsync(cartItemId))
            .ReturnsAsync(cartItem);

        _mockCartItemRepository.Setup(x => x.Remove(It.IsAny<CartItem>()));

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _cartService.RemoveFromCartAsync(cartItemId);

        // Assert
        result.Should().BeTrue();

        _mockCartItemRepository.Verify(x => x.Remove(cartItem), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RemoveFromCartAsync_WhenCartItemNotFound_ShouldReturnFalse()
    {
        // Arrange
        var cartItemId = 999;

        _mockCartItemRepository.Setup(x => x.GetByIdAsync(cartItemId))
            .ReturnsAsync((CartItem?)null);

        // Act
        var result = await _cartService.RemoveFromCartAsync(cartItemId);

        // Assert
        result.Should().BeFalse();

        _mockCartItemRepository.Verify(x => x.Remove(It.IsAny<CartItem>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ClearCartAsync_WhenCartHasItems_ShouldRemoveAllItems()
    {
        // Arrange
        var sessionId = "test-session";
        var cartItems = new List<CartItem>
        {
            new CartItem { Id = 1 },
            new CartItem { Id = 2 }
        };

        var cart = new Cart
        {
            Id = 1,
            SessionId = sessionId,
            CartItems = cartItems
        };

        _mockCartRepository.Setup(x => x.GetCartWithItemsAsync(sessionId))
            .ReturnsAsync(cart);

        _mockCartItemRepository.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<CartItem>>()));

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _cartService.ClearCartAsync(sessionId);

        // Assert
        _mockCartItemRepository.Verify(x => x.RemoveRange(cartItems), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ClearCartAsync_WhenCartIsEmpty_ShouldNotRemoveAnything()
    {
        // Arrange
        var sessionId = "test-session";
        var cart = new Cart
        {
            Id = 1,
            SessionId = sessionId,
            CartItems = new List<CartItem>()
        };

        _mockCartRepository.Setup(x => x.GetCartWithItemsAsync(sessionId))
            .ReturnsAsync(cart);

        // Act
        await _cartService.ClearCartAsync(sessionId);

        // Assert
        _mockCartItemRepository.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<CartItem>>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }
} 