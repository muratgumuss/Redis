using RedisExample.Api.Models;
using RedisExample.Api.Repository;

namespace RedisExample.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Products> CreateProduct(Products product)
        {
           return await _productRepository.CreateProduct(product);
        }

        public async Task<List<Products>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public Task<Products> GetProductByIdAsync(int id)
        {
            return _productRepository.GetProductByIdAsync(id);
        }
    }
}
