using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = redisService.GetDatabase(0);
        }
        public IActionResult Index()
        {
            db.StringSet("name", "Murat Gümüş");
            db.StringSet("visitor", "100");

            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name");
            db.StringIncrement("visitor", 10);
            //var count = db.StringDecrementAsync("visitor", 2).Result;
            db.StringDecrementAsync("visitor", 2).Wait();

            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
            }

            return View();
        }
    }
}
