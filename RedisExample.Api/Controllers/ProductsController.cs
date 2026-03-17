using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisApp.Cache;
using RedisExample.Api.Models;
using RedisExample.Api.Repository;
using RedisExample.Api.Services;
using StackExchange.Redis;

namespace RedisExample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //private readonly IProductRepository _productRepository;
        //private readonly RedisService _redisService;
        //private readonly IDatabase _database;
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        //public ProductsController(IProductRepository productRepository, RedisService redisService,IDatabase database)
        //{
        //    _productRepository = productRepository;
        //    _redisService = redisService;
        //    _database = database;
        //    //_database.StringSet("test2","Redis Connected");
        //    //var db = _redisService.GetDatabase(0);
        //    //db.StringSet("test", "Hello Redis!");
        //}



        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Products product)
        {
            return Created(string.Empty, await _productService.CreateProduct(product));
        }
    }
}
