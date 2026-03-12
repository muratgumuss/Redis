using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            _distributedCache.SetString("Name", "Murat", cacheOptions);
            await _distributedCache.SetStringAsync("Surname", "Gümüş", cacheOptions);

            Product product = new Product { Id = 2, Name = "Kalem2", Price = 100 };

            string jsonproduct = JsonConvert.SerializeObject(product);
            await _distributedCache.SetStringAsync("product:2", jsonproduct, cacheOptions);

            //binary 
            Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct);
            _distributedCache.Set("product:1", byteproduct, cacheOptions);

            return View();
        }

        public IActionResult Show()
        {
            var name = _distributedCache.GetString("Name");
            ViewBag.Name = name;

            var productJson = _distributedCache.GetString("product:2");
            var product = JsonConvert.DeserializeObject<Product>(productJson);
            ViewBag.ProductJson = product;

            Byte[] byteProduct = _distributedCache.Get("product:1");

            string jsonproduct = Encoding.UTF8.GetString(byteProduct);

            Product p = JsonConvert.DeserializeObject<Product>(jsonproduct);

            ViewBag.ByteProduct = p;

            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("Name");
            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] resimbyte = _distributedCache.Get("resim");

            return File(resimbyte, "image/jpg");
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/download.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("resim", imageByte);

            return View();
        }
    }
}