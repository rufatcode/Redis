using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Redis_Cashing.Services;
using StackExchange.Redis;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Redis_Cashing.Controllers
{
    [Route("api/[controller]")]
    public class SentinelController : Controller
    {
        [HttpPost]
        public async Task<IActionResult>Post(string key,string value)
        {
            IDatabase data =await RedisService.RedisMasterDatabase();
           bool isSuccess= await data.StringSetAsync(key, value, TimeSpan.FromMinutes(2));
            if (!isSuccess) return BadRequest();
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult>Get(string key)
        {
            IDatabase database =await RedisService.RedisMasterDatabase();
            string value =await database.StringGetAsync(key);
            return Ok(value);
        }
    }
}

