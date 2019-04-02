using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQClientPerformance.Models;
using RabbitMQClientPerformance.RabbitMQ;

namespace RabbitMQClientPerformance.Controllers
{
    public class HomeController : Controller
    {
        IRabbitMQClient _rabbitMQ;

        public HomeController(IRabbitMQClient rabbitMQ) 
        {
            _rabbitMQ = rabbitMQ;
        }

        public IActionResult Index()
        {
            var result = _rabbitMQ.Post(generateData());

            return Ok(result);
        }

        private MyData generateData() {
            return new MyData() { ID = Guid.NewGuid().ToString(), Name = "Name", Value = 1.0M };
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
