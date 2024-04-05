using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Redis_Cashing.Controllers
{
    [Route("api/[controller]")]
    public class DistributedCachingController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public DistributedCachingController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpPost]
        public async Task<IActionResult>Set(string name ,string sureName)
        {
            await _distributedCache.SetStringAsync("name", name, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(2),
                SlidingExpiration = TimeSpan.FromSeconds(20)

            }) ;
            await _distributedCache.SetAsync("sureName", Encoding.UTF8.GetBytes(sureName), options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(2),
                SlidingExpiration=TimeSpan.FromSeconds(20)
            }) ;
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Get(string key)
        {
            byte[] value = await _distributedCache.GetAsync(key);
            if (value==null)
            {
                return BadRequest();
            }
            return Ok(Encoding.UTF8.GetString(value));
        }
    }
}

