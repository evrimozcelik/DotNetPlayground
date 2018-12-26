using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DefaultRedisSession.Web.Models;
using Microsoft.AspNetCore.Http;

namespace DefaultRedisSession.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok($"DefaultRedisSession.Web is running");
        }


        [HttpPost("/api/set")]
        public async Task<IActionResult> Set(string key, string value) 
        {
            await HttpContext.Session.LoadAsync();
            HttpContext.Session.SetString(key, value);

            return Ok($"set successful, key: {key}, value: {value}");
        }

        [HttpGet("/api/get")]
        public async Task<IActionResult> Get(string key, string value)
        {
            await HttpContext.Session.LoadAsync();
            string sessionValue = HttpContext.Session.GetString(key);

            return Ok($"get successful, key: {key}, sessionValue: {sessionValue}");
        }

        [HttpGet("/api/check")]
        public async Task<IActionResult> Check(string key, string value)
        {
            await HttpContext.Session.LoadAsync();
            string sessionValue = HttpContext.Session.GetString(key);

            if(sessionValue == value) 
            {
                return Ok($"check successful, key: {key}, value: {value}, sessionValue: {sessionValue}");
            } 
            else
            {
                return NotFound($"check unsuccessful, key: {key}, value: {value}, sessionValue: {sessionValue}");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
