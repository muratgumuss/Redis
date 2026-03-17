using Microsoft.EntityFrameworkCore;
using RedisExample.Api.Models;

namespace RedisExample.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Products> CreateProduct(Products product)
        {
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Products> GetProductByIdAsync(int id)
        {
            return await _appDbContext.Products.FindAsync(id);
        }

        public async Task<List<Products>> GetAllProductsAsync()
        {
            return await _appDbContext.Products.ToListAsync();
        }
    }
}
