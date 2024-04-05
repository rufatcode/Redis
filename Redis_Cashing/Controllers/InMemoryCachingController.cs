using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Redis_Cashing.Controllers
{
    [Route("api/[controller]")]
    public class InMemoryCaching : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public InMemoryCaching(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        [HttpPost]
        public void Set(string key,string value)
        {
            _memoryCache.Set(key, value);
        }
        [HttpGet]
        public string Get(string key)
        {
            if (!_memoryCache.TryGetValue<string>(key, out string value))
            {
                return "empty cash";
            }
            return value;
        }
        [HttpPost("SetDate")]
        public void SetDate()
        {
            _memoryCache.Set<DateTime>("Birthday", DateTime.Now, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                SlidingExpiration = TimeSpan.FromSeconds(10)
            });
        }
        [HttpGet("GetDate")]
        public async Task<IActionResult> GetDate()
        {
            if (!_memoryCache.TryGetValue<DateTime>("Birthday",out DateTime date))
            {
                return Ok("Empty Cache");
            }
            return Ok(date);
        }

    }
}

