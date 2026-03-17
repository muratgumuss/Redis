using RedisExample.Api.Models;

namespace RedisExample.Api.Repository
{
    public interface IProductRepository
    {
        Task<List<Products>> GetAllProductsAsync();
        Task<Products> GetProductByIdAsync(int id);
        Task<Products> CreateProduct(Products product);
    }
}
