using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using ECommerceFurniture.DataAccess;

namespace ECommerceFurniture.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceFurnitureDbContext _context;
        private IDbContextTransaction? _transaction;

        private IProductRepository? _products;
        private ICategoryRepository? _categories;
        private ICartRepository? _carts;
        private ICartItemRepository? _cartItems;

        public UnitOfWork(ECommerceFurnitureDbContext context)
        {
            _context = context;
        }

        public IProductRepository Products =>
            _products ??= new ProductRepository(_context);

        public ICategoryRepository Categories =>
            _categories ??= new CategoryRepository(_context);

        public ICartRepository Carts =>
            _carts ??= new CartRepository(_context);

        public ICartItemRepository CartItems =>
            _cartItems ??= new CartItemRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
} 