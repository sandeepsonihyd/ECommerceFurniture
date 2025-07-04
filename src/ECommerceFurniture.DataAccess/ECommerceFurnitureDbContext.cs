using Microsoft.EntityFrameworkCore;
using ECommerceFurniture.Domain;

namespace ECommerceFurniture.DataAccess
{
    public class ECommerceFurnitureDbContext : DbContext
    {
        public ECommerceFurnitureDbContext(DbContextOptions<ECommerceFurnitureDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SKU).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.SKU).IsUnique();
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                
                entity.HasOne(e => e.Category)
                    .WithMany(e => e.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Category Configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(500);
                
                entity.HasOne(e => e.ParentCategory)
                    .WithMany(e => e.SubCategories)
                    .HasForeignKey(e => e.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Cart Configuration
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SessionId).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.SessionId);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            });

            // CartItem Configuration
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                
                entity.HasOne(e => e.Cart)
                    .WithMany(e => e.CartItems)
                    .HasForeignKey(e => e.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ProductImage Configuration
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.AltText).HasMaxLength(255);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                
                entity.HasOne(e => e.Product)
                    .WithMany(e => e.ProductImages)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ProductSpecification Configuration
            modelBuilder.Entity<ProductSpecification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Value).IsRequired().HasMaxLength(500);
                
                entity.HasOne(e => e.Product)
                    .WithMany(e => e.Specifications)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Living Room", Description = "Living room furniture", IsActive = true },
                new Category { Id = 2, Name = "Bedroom", Description = "Bedroom furniture", IsActive = true },
                new Category { Id = 3, Name = "Dining Room", Description = "Dining room furniture", IsActive = true },
                new Category { Id = 4, Name = "Office", Description = "Office furniture", IsActive = true },
                new Category { Id = 5, Name = "Sofas", Description = "All types of sofas", IsActive = true, ParentCategoryId = 1 },
                new Category { Id = 6, Name = "Tables", Description = "All types of tables", IsActive = true, ParentCategoryId = 1 }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Modern 3-Seater Sofa",
                    Description = "Comfortable and stylish 3-seater sofa perfect for modern living rooms",
                    Price = 899.99m,
                    SKU = "SOFA-MOD-001",
                    CategoryId = 5,
                    IsActive = true,
                    StockQuantity = 15,
                    CreatedDate = DateTime.UtcNow
                },
                new Product
                {
                    Id = 2,
                    Name = "Glass Coffee Table",
                    Description = "Elegant glass coffee table with metal legs",
                    Price = 299.99m,
                    SKU = "TABLE-GLASS-001",
                    CategoryId = 6,
                    IsActive = true,
                    StockQuantity = 25,
                    CreatedDate = DateTime.UtcNow
                },
                new Product
                {
                    Id = 3,
                    Name = "Executive Office Chair",
                    Description = "High-back executive chair with ergonomic design",
                    Price = 549.99m,
                    SKU = "CHAIR-EXEC-001",
                    CategoryId = 4,
                    IsActive = true,
                    StockQuantity = 10,
                    CreatedDate = DateTime.UtcNow
                },
                new Product
                {
                    Id = 4,
                    Name = "Queen Size Bed Frame",
                    Description = "Solid wood queen size bed frame",
                    Price = 699.99m,
                    SKU = "BED-QUEEN-001",
                    CategoryId = 2,
                    IsActive = true,
                    StockQuantity = 8,
                    CreatedDate = DateTime.UtcNow
                }
            );

            // Seed Product Images
            modelBuilder.Entity<ProductImage>().HasData(
                new ProductImage { Id = 1, ProductId = 1, ImageUrl = "/images/sofa-modern-001-1.jpg", AltText = "Modern 3-Seater Sofa - Front View", IsPrimary = true, DisplayOrder = 1, CreatedDate = DateTime.UtcNow },
                new ProductImage { Id = 2, ProductId = 1, ImageUrl = "/images/sofa-modern-001-2.jpg", AltText = "Modern 3-Seater Sofa - Side View", IsPrimary = false, DisplayOrder = 2, CreatedDate = DateTime.UtcNow },
                new ProductImage { Id = 3, ProductId = 2, ImageUrl = "/images/table-glass-001-1.jpg", AltText = "Glass Coffee Table - Main View", IsPrimary = true, DisplayOrder = 1, CreatedDate = DateTime.UtcNow },
                new ProductImage { Id = 4, ProductId = 3, ImageUrl = "/images/chair-exec-001-1.jpg", AltText = "Executive Office Chair - Front View", IsPrimary = true, DisplayOrder = 1, CreatedDate = DateTime.UtcNow },
                new ProductImage { Id = 5, ProductId = 4, ImageUrl = "/images/bed-queen-001-1.jpg", AltText = "Queen Size Bed Frame - Main View", IsPrimary = true, DisplayOrder = 1, CreatedDate = DateTime.UtcNow }
            );

            // Seed Product Specifications
            modelBuilder.Entity<ProductSpecification>().HasData(
                new ProductSpecification { Id = 1, ProductId = 1, Name = "Dimensions", Value = "84\" W x 36\" D x 32\" H", DisplayOrder = 1 },
                new ProductSpecification { Id = 2, ProductId = 1, Name = "Material", Value = "Premium Fabric, Hardwood Frame", DisplayOrder = 2 },
                new ProductSpecification { Id = 3, ProductId = 1, Name = "Color", Value = "Charcoal Gray", DisplayOrder = 3 },
                new ProductSpecification { Id = 4, ProductId = 2, Name = "Dimensions", Value = "48\" W x 24\" D x 16\" H", DisplayOrder = 1 },
                new ProductSpecification { Id = 5, ProductId = 2, Name = "Material", Value = "Tempered Glass, Stainless Steel", DisplayOrder = 2 },
                new ProductSpecification { Id = 6, ProductId = 3, Name = "Dimensions", Value = "26\" W x 28\" D x 44-48\" H", DisplayOrder = 1 },
                new ProductSpecification { Id = 7, ProductId = 3, Name = "Material", Value = "Genuine Leather, Aluminum Base", DisplayOrder = 2 },
                new ProductSpecification { Id = 8, ProductId = 4, Name = "Dimensions", Value = "64\" W x 84\" L x 14\" H", DisplayOrder = 1 },
                new ProductSpecification { Id = 9, ProductId = 4, Name = "Material", Value = "Solid Oak Wood", DisplayOrder = 2 }
            );
        }
    }
} 