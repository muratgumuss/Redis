using RedisApp.Cache;
using RedisExample.Api.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExample.Api.Repository
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly IDatabase _cacheRepository;
        private readonly RedisService _redisService;
        private const string productKey = "productCaches";

        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheRepository = redisService.GetDatabase(3);
        }

        public async Task<Products> CreateProduct(Products product)
        {
            var newProduct = await _productRepository.CreateProduct(product);

            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                await _cacheRepository.HashSetAsync(productKey, newProduct.Id, JsonSerializer.Serialize(newProduct));
            }

            return newProduct;
        }

        public async Task<List<Products>> GetAllProductsAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
            {
                return await LoadToCacheFromDbAsync();
            }

            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);
            var products = new List<Products>();

            foreach (var item in cacheProducts.ToList())
            {
                var product = JsonSerializer.Deserialize<Products>((string)item.Value);
                products.Add(product);
            }

            return products;
        }

        public async Task<Products> GetProductByIdAsync(int id)
        {
            if (_cacheRepository.KeyExists(productKey))
            {
                RedisValue product = await _cacheRepository.HashGetAsync(productKey, id);

                return product.HasValue ? JsonSerializer.Deserialize<Products>(product.ToString()) : null;
            }

            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(p => p.Id == id);
        }

        private async Task<List<Products>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();

            products.ForEach(p =>
            {
                _cacheRepository.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });

            return products;
        }
    }
}
