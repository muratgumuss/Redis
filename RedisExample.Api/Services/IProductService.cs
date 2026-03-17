using RedisExample.Api.Models;

namespace RedisExample.Api.Services
{
    public interface IProductService
    {
        Task<List<Products>> GetAllProductsAsync();
        Task<Products> GetProductByIdAsync(int id);
        Task<Products> CreateProduct(Products product);
    }
}
